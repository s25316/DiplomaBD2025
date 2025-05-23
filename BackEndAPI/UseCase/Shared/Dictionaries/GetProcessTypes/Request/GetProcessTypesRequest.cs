using MediatR;
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;

namespace UseCase.Shared.Dictionaries.GetProcessTypes.Request
{
    public class GetProcessTypesRequest : IRequest<IEnumerable<ProcessTypeDto>>
    {
    }
}
