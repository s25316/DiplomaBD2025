using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitmentMessages.Request
{
    public class GetPersonRecruitmentMessagesRequest : BaseRequest<ItemsResponse<MessageDto>>
    {
        public required Guid RecruitmentId { get; init; }
        public required PaginationQueryParametersDto PaginationQueryParameters { get; init; }
        public required bool Ascending { get; init; }
    }
}
