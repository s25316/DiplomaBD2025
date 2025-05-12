// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.Responses.BaseResponses;

namespace UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Response
{
    public class CompanyAndOfferTemplateDto
    {
        public required CompanyDto Company { get; init; }

        public required OfferTemplateDto OfferTemplate { get; init; }
    }
}
