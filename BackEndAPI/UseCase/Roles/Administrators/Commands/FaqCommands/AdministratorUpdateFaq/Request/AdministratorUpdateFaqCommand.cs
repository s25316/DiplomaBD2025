using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorUpdateFaq.Request
{
    public class AdministratorUpdateFaqCommand
    {
        [Required]
        public required string Answer { get; init; }
    }
}
