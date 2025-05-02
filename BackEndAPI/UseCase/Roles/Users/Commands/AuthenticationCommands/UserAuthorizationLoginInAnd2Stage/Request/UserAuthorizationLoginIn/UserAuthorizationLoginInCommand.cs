using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorizationLoginIn
{
    public class UserAuthorizationLoginInCommand
    {
        [Required]
        [EmailAddress]
        public required string Login { get; init; }

        [Required]
        public required string Password { get; init; }
    }
}
