using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.BaseResponses.Administrator;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetAdministrators.Request
{
    public class AdministratorGetAdministratorsRequest : GetPeopleRequest<ItemsResponse<AdministratorPersonProfile>>
    {
    }
}
