using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorCreateFaq.Request
{
    public class AdministratorCreateFaqCommand
    {
        [Required]
        public required string Question { get; init; }

        [Required]
        public required string Answer { get; init; }
    }
}
