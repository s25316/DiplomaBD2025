// Ignore Spelling: Dto

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser
{
    public class GuestBranchDto
    {
        public Guid BranchId { get; init; }

        public Guid CompanyId { get; init; }

        public Guid AddressId { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }

        public DateTime Created { get; init; }

        public AddressResponseDto Address { get; init; } = null!;
    }
}
