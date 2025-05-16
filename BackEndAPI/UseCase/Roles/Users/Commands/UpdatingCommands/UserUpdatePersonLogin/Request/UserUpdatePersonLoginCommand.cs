using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonLogin.Request
{
    public class UserUpdatePersonLoginCommand
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; init; }
    }
}
