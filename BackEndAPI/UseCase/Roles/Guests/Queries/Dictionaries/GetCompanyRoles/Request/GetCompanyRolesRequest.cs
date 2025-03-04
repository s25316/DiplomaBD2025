using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCompanyRoles.Request
{
    public class GetCompanyRolesRequest : IRequest<IEnumerable<CompanyRoleDto>>
    {
    }
}
