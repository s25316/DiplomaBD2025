using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserInitiateUpdatePersonPassword.Request
{
    public class UserInitiateUpdatePersonPasswordCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; }
    }
}
