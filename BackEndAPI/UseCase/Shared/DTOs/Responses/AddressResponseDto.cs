namespace UseCase.Shared.DTOs.Responses
{
    public class AddressResponseDto
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; } = null!;

        public int StateId { get; set; }

        public string StateName { get; set; } = null!;

        public int CityId { get; set; }

        public string CityName { get; set; } = null!;

        public int? StreetId { get; set; }

        public string? StreetName { get; set; } = null!;

        public string HouseNumber { get; set; } = null!;

        public string? ApartmentNumber { get; set; }

        public string PostCode { get; set; } = null!;

        public float Lon { get; set; }

        public float Lat { get; set; }
    }
}
