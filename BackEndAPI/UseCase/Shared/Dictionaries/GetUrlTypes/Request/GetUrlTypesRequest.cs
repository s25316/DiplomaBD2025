using MediatR;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;

namespace UseCase.Shared.Dictionaries.GetUrlTypes.Request
{
    public class GetUrlTypesRequest : IRequest<IEnumerable<UrlTypeDto>>
    {
    }
}
