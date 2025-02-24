using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetSalaryTerms.Request
{
    public class GetSalaryTermsRequest : IRequest<IEnumerable<SalaryTermDto>>
    {
    }
}
