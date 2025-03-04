using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCurrencies.Request
{
    public class GetCurrenciesRequest : IRequest<IEnumerable<CurrencyDto>>
    {
    }
}
