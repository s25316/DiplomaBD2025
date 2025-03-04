using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response
{
    public class GetPersonCompaniesResponse
    {
        public required IEnumerable<CompanyDto> Companies { get; init; } = [];
    }
}
