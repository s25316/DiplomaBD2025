using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request
{
    public class UserProfileCreateCommand
    {
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
    }
}
