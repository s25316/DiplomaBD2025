using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response
{
    public class CompanyAndOfferTemplate
    {
        public required CompanyDto Company { get; init; }
        public required OfferTemplateDto OfferTemplate { get; init; }
    }
}
