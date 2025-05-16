// Ignore Spelling: Dto, JWt
namespace UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses
{
    public class UserAuthorizationDataDto
    {
        public required string Jwt { get; init; }
        public DateTime JwtValidTo { get; init; }
        public required string RefreshToken { get; init; }
        public DateTime RefreshTokenValidTo { get; init; }
    }
}
