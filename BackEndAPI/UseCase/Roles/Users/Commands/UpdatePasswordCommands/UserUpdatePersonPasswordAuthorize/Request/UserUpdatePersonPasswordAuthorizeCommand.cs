using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordAuthorize.Request
{
    public class UserUpdatePersonPasswordAuthorizeCommand
    {
        [Required]
        public required string OldPassword { get; init; }
        [Required]
        public required string NewPassword { get; init; }
    }
}
