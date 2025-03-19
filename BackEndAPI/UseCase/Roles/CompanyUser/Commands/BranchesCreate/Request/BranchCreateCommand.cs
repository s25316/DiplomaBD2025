using System.ComponentModel.DataAnnotations;
using UseCase.Shared.DTOs.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request
{
    public class BranchCreateCommand
    {
        [Required]
        public AddressRequestDto Address { get; init; } = null!;

        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;
    }
}
