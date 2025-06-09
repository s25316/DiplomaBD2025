using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorBlockPerson.Request
{
    public class AdministratorBlockPersonCommand
    {
        [Required]
        public required string Message { get; init; }
    }
}
