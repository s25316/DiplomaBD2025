// Ignore Spelling: Dto
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser
{
    public class CompanyUserOfferTemplateDto : GuestOfferTemplateDto
    {
        public DateTime? Removed { get; init; }
    }
}
