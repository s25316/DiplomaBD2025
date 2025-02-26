// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Requests
{
    public class AddressRequestDto
    {
        public string CountryName { get; init; } = null!;
        public string StateName { get; init; } = null!;
        public string CityName { get; init; } = null!;
        public string? StreetName { get; init; } = null!;
        public string HouseNumber { get; init; } = null!;
        public string? ApartmentNumber { get; init; } = null!;
        public string PostCode { get; init; } = null!;
        public float Lon { get; init; }
        public float Lat { get; init; }
    }
}
