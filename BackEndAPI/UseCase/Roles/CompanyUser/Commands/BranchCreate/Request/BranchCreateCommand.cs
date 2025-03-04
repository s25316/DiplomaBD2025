using UseCase.Shared.DTOs.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate.Request
{
    public record BranchCreateCommand
    {
        //[Required]
        public required string Name { get; init; } = null!;
        public string? Description { get; init; } = null;
        //[Required]
        public required AddressRequestDto Address { get; init; } = null!;
    }
}
