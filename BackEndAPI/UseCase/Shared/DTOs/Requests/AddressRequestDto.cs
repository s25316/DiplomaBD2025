// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Requests
{
    public class AddressRequestDto
    {
        public string CountryName { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string CityName { get; set; } = null!;
        public string? StreetName { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public float Lon { get; set; }
        public float Lat { get; set; }
    }
}
