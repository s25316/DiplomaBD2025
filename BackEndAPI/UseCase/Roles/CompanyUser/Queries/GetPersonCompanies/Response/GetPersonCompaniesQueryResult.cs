// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response
{
    public class GetPersonCompaniesQueryResult
    {
        public required IEnumerable<CompanyDto> Items { get; init; } = [];
        public int TotalCount { get; init; } = 0;
    }
}
