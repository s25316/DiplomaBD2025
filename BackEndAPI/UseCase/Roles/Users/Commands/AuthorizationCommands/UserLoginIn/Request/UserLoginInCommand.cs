using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Request
{
    public class UserLoginInCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; }

        [Required]
        public required string Password { get; init; }
    }
}
