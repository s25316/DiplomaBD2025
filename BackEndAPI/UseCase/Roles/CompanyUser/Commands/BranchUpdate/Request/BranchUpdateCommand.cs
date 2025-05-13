using UseCase.Shared.Requests.DTOs;

namespace UseCase.Roles.CompanyUser.Commands.BranchUpdate.Request
{
    public class BranchUpdateCommand
    {
        public AddressRequestDto? Address { get; init; } = null;

        public string? Name { get; init; } = null;

        public string? Description { get; init; } = null;
    }
}
