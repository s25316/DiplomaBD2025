using MediatR;
using UseCase.Shared.Dictionaries.GetFaqs.Response;

namespace UseCase.Shared.Dictionaries.GetFaqs.Request
{
    public class GetFaqsRequest : IRequest<IEnumerable<FaqDto>>
    {
    }
}
