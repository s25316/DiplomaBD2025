// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses
{
    public class AddressResponseDto
    {
        public Guid AddressId { get; init; }
        public int CountryId { get; init; }
        public string CountryName { get; init; } = null!;
        public int StateId { get; init; }
        public string StateName { get; init; } = null!;
        public int CityId { get; init; }
        public string CityName { get; init; } = null!;
        public int? StreetId { get; init; }
        public string? StreetName { get; init; } = null!;
        public string HouseNumber { get; init; } = null!;
        public string? ApartmentNumber { get; init; } = null!;
        public string PostCode { get; init; } = null!;
        public float Lon { get; init; }
        public float Lat { get; init; }
    }
}
