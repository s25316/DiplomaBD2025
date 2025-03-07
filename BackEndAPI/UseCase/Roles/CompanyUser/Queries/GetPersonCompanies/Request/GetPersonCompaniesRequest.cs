// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request
{
    public class GetPersonCompaniesRequest : RequestTemplate<GetPersonCompaniesResponse>
    {
        public Guid? CompanyId { get; init; }
        public string? SearchText { get; init; } = null;
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }
        public required CompaniesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
