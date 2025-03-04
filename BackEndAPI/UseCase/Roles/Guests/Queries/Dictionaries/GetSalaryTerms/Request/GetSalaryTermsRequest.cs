using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSalaryTerms.Request
{
    public class GetSalaryTermsRequest : IRequest<IEnumerable<SalaryTermDto>>
    {
    }
}
