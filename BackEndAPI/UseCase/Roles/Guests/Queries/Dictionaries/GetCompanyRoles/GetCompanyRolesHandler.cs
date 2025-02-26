using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetCompanyRoles.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetCompanyRoles
{
    public class GetCompanyRolesHandler : IRequestHandler<GetCompanyRolesRequest, IEnumerable<CompanyRoleDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetCompanyRolesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<CompanyRoleDto>> Handle(GetCompanyRolesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetCompanyRolesAsync();
            return dictionary.Values.OrderBy(value => value.RoleId);
        }
    }
}
