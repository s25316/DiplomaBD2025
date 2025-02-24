using System.ComponentModel.DataAnnotations;
using UseCase.Roles.Users.Commands.UserLoginIn.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserLoginIn.Request
{
    public class UserLoginInRequest : BaseRequest<UserLoginInResponse>
    {
        [Required]
        public required UserLoginInCommand Command { get; init; } = null!;
    }
}
