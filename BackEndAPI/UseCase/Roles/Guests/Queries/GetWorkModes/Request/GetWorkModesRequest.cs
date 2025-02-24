using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetWorkModes.Request
{
    public class GetWorkModesRequest : IRequest<IEnumerable<WorkModeDto>>
    {
    }
}
