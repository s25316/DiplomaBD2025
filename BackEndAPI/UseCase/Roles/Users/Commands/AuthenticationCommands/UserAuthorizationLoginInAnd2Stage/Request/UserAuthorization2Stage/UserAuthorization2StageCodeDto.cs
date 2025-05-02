// Ignore Spelling: Dto
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage
{
    public class UserAuthorization2StageCodeDto
    {
        [Required]
        public required string Code { get; init; }
    }
}
