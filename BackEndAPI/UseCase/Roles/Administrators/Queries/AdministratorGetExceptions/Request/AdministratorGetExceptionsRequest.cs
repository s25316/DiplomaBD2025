using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.BaseResponses.Administrator;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetExceptions.Request
{
    public class AdministratorGetExceptionsRequest : BaseRequest<ItemsResponse<ExceptionDto>>
    {
        public required PaginationQueryParametersDto PaginationQueryParameters { get; init; }
        public required bool Ascending { get; init; }
    }
}
