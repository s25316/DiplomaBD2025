using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UserRegistration.Request
{
    public class UserRegistrationCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; } = null!;

        [Required]
        public required string Password { get; init; } = null!;
    }
}
