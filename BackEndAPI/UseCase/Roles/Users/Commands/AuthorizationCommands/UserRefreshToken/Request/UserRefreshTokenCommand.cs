using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserRefreshToken.Request
{
    public class UserRefreshTokenCommand
    {
        [Required]
        public required string RefreshToken { get; init; }
    }
}
