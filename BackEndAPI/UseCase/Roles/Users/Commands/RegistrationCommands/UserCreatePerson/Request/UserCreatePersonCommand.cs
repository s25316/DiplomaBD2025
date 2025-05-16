using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.RegistrationCommands.UserCreatePerson.Request
{
    public class UserCreatePersonCommand
    {
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
    }
}
