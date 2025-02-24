using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetCurrencies.Request
{
    public class GetCurrenciesRequest : IRequest<IEnumerable<CurrencyDto>>
    {
    }
}
