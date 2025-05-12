// Ignore Spelling: Dto

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser
{
    public class GuestBranchDto
    {
        public Guid BranchId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid AddressId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public AddressResponseDto Address { get; set; } = null!;
    }
}
