using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserSetBasePersonData.Request
{
    public class UserSetBasePersonDataCommand
    {
        [Required]
        public required string Name { get; init; }

        [Required]
        public required string Surname { get; init; }

        public DateTime? BirthDate { get; init; }
    }
}
