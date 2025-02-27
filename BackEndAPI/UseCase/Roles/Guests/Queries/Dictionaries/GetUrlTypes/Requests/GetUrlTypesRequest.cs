using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetUrlTypes.Requests
{
    public class GetUrlTypesRequest : IRequest<IEnumerable<UrlTypeDto>>
    {
    }
}
