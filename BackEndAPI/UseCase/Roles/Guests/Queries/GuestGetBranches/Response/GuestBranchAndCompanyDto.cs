using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.Guests.Queries.GuestGetBranches.Response
{
    public class GuestBranchAndCompanyDto
    {
        public required CompanyDto Company { get; init; }

        public required GuestBranchDto Branch { get; init; }

        public required int OfferCount { get; init; }
    }
}
