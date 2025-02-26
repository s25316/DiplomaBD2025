using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCompanyRoles.Request
{
    public class GetCompanyRolesRequest : IRequest<IEnumerable<CompanyRoleDto>>
    {
    }
}
