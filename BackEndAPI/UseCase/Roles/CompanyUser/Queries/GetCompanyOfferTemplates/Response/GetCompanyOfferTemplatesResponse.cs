using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response
{
    public class GetCompanyOfferTemplatesResponse
    {
        public required IEnumerable<OfferTemplateDto> OfferTemplates { get; init; } = [];
    }
}
