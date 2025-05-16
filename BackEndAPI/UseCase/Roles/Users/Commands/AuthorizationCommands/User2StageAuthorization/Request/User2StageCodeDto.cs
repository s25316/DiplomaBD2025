// Ignore Spelling: Dto
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization.Request
{
    public class User2StageCodeDto
    {
        [Required]
        public required string Code { get; init; }
    }
}
