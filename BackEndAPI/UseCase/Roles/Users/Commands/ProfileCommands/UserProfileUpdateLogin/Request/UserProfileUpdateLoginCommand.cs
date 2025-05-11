using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileUpdateLogin.Request
{
    public class UserProfileUpdateLoginCommand
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; init; }
    }
}
