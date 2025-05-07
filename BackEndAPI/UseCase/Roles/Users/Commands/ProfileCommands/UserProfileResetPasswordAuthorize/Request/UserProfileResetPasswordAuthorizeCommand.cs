using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request
{
    public class UserProfileResetPasswordAuthorizeCommand
    {
        [Required]
        public required string OldPassword { get; init; }
        [Required]
        public required string NewPassword { get; init; }
    }
}
