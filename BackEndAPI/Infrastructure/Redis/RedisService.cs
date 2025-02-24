// Ignore Spelling: redis

using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services.Time;

namespace Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        // Properties
        private readonly IDatabase _database;
        private readonly ITimeService _timeService;

        private static int _countHoursExpiration = 2;
        private static string _host = UseCase.Configuration.RedisConnectionString
            .Split(",")[0].Split(":")[0].Trim();
        private static int _port = int.Parse(UseCase.Configuration.RedisConnectionString
            .Split(",")[0].Split(":")[1]);


        // Constructor
        public RedisService(
            ITimeService timeService,
            IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _timeService = timeService;
        }

        //Methods
        public async Task SetAsync(Dictionary<string, object> dictionary)
        {
            if (!dictionary.Any())
            {
                return;
            }

            var unixTimestamp = (long)(
                _timeService.GetNow().AddHours(_countHoursExpiration).ToUniversalTime() - DateTime.UnixEpoch
                ).TotalSeconds;

            foreach (var pair in dictionary)
            {
                var key = $"{pair.Value.GetType().Name}:{pair.Key}";
                var value = JsonSerializer.Serialize(pair.Value);

                await _database.StringSetAsync(key, value);
                await _database.KeyExpireAsync(key, DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime);
            }
        }

        public async Task<IEnumerable<T>> GetAsync<T>() where T : class
        {
            var server = _database.Multiplexer.GetServer(_host, _port);
            var keys = server.Keys(pattern: $"{typeof(T).Name}:*").ToList();

            if (!keys.Any())
            {
                return Enumerable.Empty<T>();
            }


            var result = new List<T>();
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
                    var deserializedValue = JsonSerializer.Deserialize<T>(json);
                    if (deserializedValue != null)
                    {
                        result.Add(deserializedValue);
                    }
                }
                catch (JsonException)
                {
                    // Obsługa błędu deserializacji JSON (opcjonalnie)
                }
            }
            return result;
        }
    }
}
