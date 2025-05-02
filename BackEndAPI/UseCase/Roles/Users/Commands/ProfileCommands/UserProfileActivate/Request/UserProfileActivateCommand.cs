using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request
{
    public class UserProfileActivateCommand
    {
        [Required]
        public required Guid UserId { get; init; }
        [Required]
        public required string ActivationUrlSegment { get; init; }
    }
}
