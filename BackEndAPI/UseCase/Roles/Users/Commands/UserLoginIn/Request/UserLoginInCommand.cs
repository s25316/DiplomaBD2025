using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UserLoginIn.Request
{
    public class UserLoginInCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; } = null!;

        [Required]
        public required string Password { get; init; } = null!;
    }
}
