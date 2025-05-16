using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordUnAuthorize.Request
{
    public class UserUpdatePersonPasswordUnAuthorizeCommand
    {
        [Required]
        public required string NewPassword { get; init; }
    }
}
