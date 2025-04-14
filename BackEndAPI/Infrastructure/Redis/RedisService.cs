// Ignore Spelling: redis Dto

using Domain.Shared.CustomProviders;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using UseCase.Shared.Interfaces;

namespace Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        // Properties
        private readonly IDatabase _database;

        private static int _countHoursExpiration = 1;
        private static string _host = UseCase.Configuration.RedisConnectionString
            .Split(",")[0].Split(":")[0].Trim();
        private static int _port = int.Parse(UseCase.Configuration.RedisConnectionString
            .Split(",")[0].Split(":")[1]);


        // Constructor
        public RedisService(
            IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        //Methods

        public async Task SetAsync<TKey, TDto>(Dictionary<TKey, TDto> dictionary)
           where TKey : notnull
           where TDto : class
        {
            if (!dictionary.Any())
            {
                return;
            }

            var universalTimeExpiration = CustomTimeProvider.Now
                .AddHours(_countHoursExpiration)
                .ToUniversalTime();
            var unixTimestamp = (long)(universalTimeExpiration - DateTime.UnixEpoch).TotalSeconds;
            var dateTimeOffsetEnd = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;

            var transaction = _database.CreateTransaction();
            foreach (var pair in dictionary)
            {
                var key = $"{typeof(TDto).Name}:{pair.Key}";
                var value = JsonSerializer.Serialize(pair.Value);

                _ = transaction.StringSetAsync(key, value)
                    .ConfigureAwait(false);
                _ = transaction.KeyExpireAsync(key, dateTimeOffsetEnd)
                    .ConfigureAwait(false);
            }
            await transaction.ExecuteAsync().ConfigureAwait(false);
        }

        public async Task<Dictionary<TKey, TDto>> GetAsync<TKey, TDto>(
            Func<TDto, TKey> keySelector)
            where TKey : notnull
            where TDto : class
        {
            var server = _database.Multiplexer.GetServer(_host, _port);
            var keys = server.Keys(pattern: $"{typeof(TDto).Name}:*").ToList();

            if (!keys.Any())
            {
                return [];
            }

            var dictionary = new Dictionary<TKey, TDto>();
            var values = await _database.StringGetAsync(keys.ToArray());

            foreach (var value in values)
            {
                if (value.IsNullOrEmpty)
                {
                    continue;
                }
                var json = Encoding.UTF8.GetString(value);
                try
                {
                    var deserializedValue = JsonSerializer.Deserialize<TDto>(json);
                    if (deserializedValue != null)
                    {
                        dictionary[keySelector(deserializedValue)] = deserializedValue;
                    }
                }
                catch (JsonException)
                {
                    // Obsługa błędu deserializacji JSON (opcjonalnie)
                }
            }
            return dictionary;
        }
    }
}
