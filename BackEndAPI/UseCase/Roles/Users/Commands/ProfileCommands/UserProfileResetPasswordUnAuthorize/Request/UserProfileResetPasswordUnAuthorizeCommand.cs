using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordUnAuthorize.Request
{
    public class UserProfileResetPasswordUnAuthorizeCommand
    {
        [Required]
        public required string NewPassword { get; init; }
    }
}
