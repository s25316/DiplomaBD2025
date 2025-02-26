// Ignore Spelling: redis

using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetCurrencies.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCurrencies
{
    public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesRequest, IEnumerable<CurrencyDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetCurrenciesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<CurrencyDto>> Handle(GetCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetCurrenciesAsync();
            return dictionary.Values.OrderBy(value => value.CurrencyId);
        }
    }
}
