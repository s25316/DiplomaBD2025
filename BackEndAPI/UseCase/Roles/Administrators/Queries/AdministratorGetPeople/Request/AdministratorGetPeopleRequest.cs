using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.BaseResponses.Administrator;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetPeople.Request
{
    public class AdministratorGetPeopleRequest : GetPeopleRequest<ItemsResponse<AdministratorPersonProfile>>
    {
    }
}
