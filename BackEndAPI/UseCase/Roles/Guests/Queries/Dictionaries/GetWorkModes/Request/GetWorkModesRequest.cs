using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetWorkModes.Request
{
    public class GetWorkModesRequest : IRequest<IEnumerable<WorkModeDto>>
    {
    }
}
