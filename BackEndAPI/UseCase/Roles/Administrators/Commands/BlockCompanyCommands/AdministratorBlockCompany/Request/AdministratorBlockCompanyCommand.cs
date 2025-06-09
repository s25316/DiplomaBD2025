using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorBlockCompany.Request
{
    public class AdministratorBlockCompanyCommand
    {
        [Required]
        public required string Message { get; init; }
    }
}
