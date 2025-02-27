using UseCase.Shared.DTOs.Requests;

namespace UseCase.Shared.Repositories.Addresses
{
    public interface IAddressRepository
    {
        Task<Guid> CreateAddressAsync(AddressRequestDto request);
    }
}
