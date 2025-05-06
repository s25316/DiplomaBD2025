// Ignore Spelling: Dto
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorization2Stage.Request
{
    public class UserAuthorization2StageCodeDto
    {
        [Required]
        public required string Code { get; init; }
    }
}
