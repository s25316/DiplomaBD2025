using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetFAQ.Request
{
    public class AdministratorGetFaqRequest : BaseRequest<ItemsResponse<FaqDto>>
    {
        public required bool? ShowRemoved { get; init; }
        public required PaginationQueryParametersDto PaginationQueryParameters { get; init; }
        public required bool Ascending { get; init; }
    }
}
