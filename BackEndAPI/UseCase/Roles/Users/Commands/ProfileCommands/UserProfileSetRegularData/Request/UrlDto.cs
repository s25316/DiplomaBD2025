// Ignore Spelling: Dto
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Request
{
    public class UrlDto
    {
        [Required]
        public required string Value { get; init; }

        [Required]
        public required int UrlTypeId { get; init; }
    }
}
