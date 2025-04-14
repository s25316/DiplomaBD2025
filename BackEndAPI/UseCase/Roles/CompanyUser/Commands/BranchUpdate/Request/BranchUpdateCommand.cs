using UseCase.Shared.DTOs.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchUpdate.Request
{
    public class BranchUpdateCommand
    {
        public AddressRequestDto? Address { get; init; } = null;

        public string? Name { get; init; } = null;

        public string? Description { get; init; } = null;
    }
}
