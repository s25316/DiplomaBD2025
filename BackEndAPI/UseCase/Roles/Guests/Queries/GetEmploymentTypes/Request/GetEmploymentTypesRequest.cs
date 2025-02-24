using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetEmploymentTypes.Request
{
    public class GetEmploymentTypesRequest : IRequest<IEnumerable<EmploymentTypeDto>>
    {
    }
}
