using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetFaqs.Request
{
    public class GetFaqsRequest : IRequest<IEnumerable<FaqDto>>
    {
    }
}
