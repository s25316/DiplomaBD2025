using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Request
{
    public class UserProfileResetPasswordInitiateCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; }
    }
}
