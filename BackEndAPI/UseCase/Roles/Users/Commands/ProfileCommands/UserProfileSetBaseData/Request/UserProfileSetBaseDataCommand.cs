using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request
{
    public class UserProfileSetBaseDataCommand
    {
        [Required]
        public required string Name { get; init; }

        [Required]
        public required string Surname { get; init; }

        public DateTime? BirthDate { get; init; }
    }
}
