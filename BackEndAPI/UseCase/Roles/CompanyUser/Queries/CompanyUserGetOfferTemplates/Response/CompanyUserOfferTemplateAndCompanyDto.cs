// Ignore Spelling: Dto

using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Response
{
    public class CompanyUserOfferTemplateAndCompanyDto
    {
        public required CompanyDto Company { get; init; }

        public required CompanyUserOfferTemplateDto OfferTemplate { get; init; }

        public required int OfferCount { get; init; }
    }
}
