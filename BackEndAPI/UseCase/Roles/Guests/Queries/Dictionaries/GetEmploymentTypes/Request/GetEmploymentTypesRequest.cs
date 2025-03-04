using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetEmploymentTypes.Request
{
    public class GetEmploymentTypesRequest : IRequest<IEnumerable<EmploymentTypeDto>>
    {
    }
}
