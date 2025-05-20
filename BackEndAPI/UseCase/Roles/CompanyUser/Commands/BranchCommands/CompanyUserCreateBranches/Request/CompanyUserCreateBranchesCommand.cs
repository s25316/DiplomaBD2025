using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Requests.DTOs;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserCreateBranches.Request
{
    public class CompanyUserCreateBranchesCommand
    {
        [Required]
        public AddressRequestDto Address { get; init; } = null!;

        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;
    }
}
