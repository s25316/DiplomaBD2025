using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.BaseResponses.User;

namespace UseCase.Roles.Users.Queries.GetPersonProfile.Response
{
    public class GetPersonProfileResponse
    {
        public required UserPersonProfile PersonPerspective { get; init; }
        public required CompanyUserPersonProfile CompanyPerspective { get; init; }
    }
}
