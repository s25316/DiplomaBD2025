using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetOffers.Response
{
    public class BranchDto
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
