// Ignore Spelling: Dto
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser
{
    public class CompanyUserContractConditionDto : GuestContractConditionDto
    {
        public DateTime? Removed { get; set; }
    }
}
