using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RegistrationCommands.UserActivatePerson.Request
{
    public class UserActivatePersonRequest : BaseRequest<ResultMetadataResponse>
    {
        public required UserActivatePersonCommand Command { get; init; }
    }
}
