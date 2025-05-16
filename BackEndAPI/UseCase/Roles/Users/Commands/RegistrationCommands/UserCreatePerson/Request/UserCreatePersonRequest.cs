using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RegistrationCommands.UserCreatePerson.Request
{
    public class UserCreatePersonRequest : BaseRequest<ResultMetadataResponse>
    {
        public required UserCreatePersonCommand Command { get; init; }
    }
}
