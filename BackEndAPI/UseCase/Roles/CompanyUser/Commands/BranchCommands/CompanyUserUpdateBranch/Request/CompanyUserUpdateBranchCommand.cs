using UseCase.Shared.Requests.DTOs;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserUpdateBranch.Request
{
    public class CompanyUserUpdateBranchCommand
    {
        public AddressRequestDto? Address { get; init; } = null;

        public string? Name { get; init; } = null;

        public string? Description { get; init; } = null;
    }
}
