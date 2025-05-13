using UseCase.Shared.Requests.DTOs;

namespace UseCase.Shared.Repositories.Addresses
{
    public interface IAddressRepository
    {
        Task<Guid> CreateAddressAsync(AddressRequestDto request);
    }
}
