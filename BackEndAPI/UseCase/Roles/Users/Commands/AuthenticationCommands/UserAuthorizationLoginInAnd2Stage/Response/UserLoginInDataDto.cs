// Ignore Spelling: Dto, JWt
namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response
{
    public class UserLoginInDataDto
    {
        public required string Jwt { get; init; }
        public DateTime JwtValidTo { get; init; }
        public required string RefreshToken { get; init; }
        public DateTime RefreshTokenValidTo { get; init; }
    }
}
