using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request
{
    public class UserAuthorizationRefreshTokenCommand
    {
        [Required]
        public required string RefreshToken { get; init; }
    }
}
