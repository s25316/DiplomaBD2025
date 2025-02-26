using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCurrencies.Request
{
    public class GetCurrenciesRequest : IRequest<IEnumerable<CurrencyDto>>
    {
    }
}
