// Ignore Spelling: Dto
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData.Request
{
    public class UrlRequestDto
    {
        [Required]
        public required string Value { get; init; }

        [Required]
        public required int UrlTypeId { get; init; }
    }
}
