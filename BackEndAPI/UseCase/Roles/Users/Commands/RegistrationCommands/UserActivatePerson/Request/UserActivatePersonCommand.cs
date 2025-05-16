using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.RegistrationCommands.UserActivatePerson.Request
{
    public class UserActivatePersonCommand
    {
        [Required]
        public required Guid UserId { get; init; }
        [Required]
        public required string ActivationUrlSegment { get; init; }
    }
}
