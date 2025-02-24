// Ignore Spelling: redis

using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetCurrencies.Request;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.GetCurrencies
{
    public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesRequest, IEnumerable<CurrencyDto>>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;


        // Constructor
        public GetCurrenciesHandler(DiplomaBdContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }


        // Methods
        public async Task<IEnumerable<CurrencyDto>> Handle(GetCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var redisList = await _redisService.GetAsync<CurrencyDto>();
            if (redisList.Any())
            {
                return redisList.OrderBy(currency => currency.CurrencyId);
            }

            var databaseList = await _context.Currencies.ToListAsync(cancellationToken);
            var result = databaseList.Select(dbCurrency => new CurrencyDto
            {
                CurrencyId = dbCurrency.CurrencyId,
                Name = dbCurrency.Name,
            });

            var dictionary = result.ToDictionary(
                item => $"{item.CurrencyId}",
                item => (object)item);
            await _redisService.SetAsync(dictionary);

            return result;
        }
    }
}
