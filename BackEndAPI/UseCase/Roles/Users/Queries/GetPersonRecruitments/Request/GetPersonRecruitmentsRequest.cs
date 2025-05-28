using UseCase.Roles.Users.Queries.GetPersonRecruitments.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitments.Request
{
    public class GetPersonRecruitmentsRequest : GetRecruitmentsRequest<ItemsResponse<UserRecruitmentDataDto>>
    {
    }
}
