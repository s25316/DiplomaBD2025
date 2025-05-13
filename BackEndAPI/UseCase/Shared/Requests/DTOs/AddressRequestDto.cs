// Ignore Spelling: Dto

using System.ComponentModel.DataAnnotations;

namespace UseCase.Shared.Requests.DTOs
{
    public record AddressRequestDto
    {
        [Required]
        public string CountryName { get; init; } = null!;

        [Required]
        public string StateName { get; init; } = null!;

        [Required]
        public string CityName { get; init; } = null!;

        public string? StreetName { get; init; } = null!;

        [Required]
        public string HouseNumber { get; init; } = null!;

        public string? ApartmentNumber { get; init; }

        [Required]
        public string PostCode { get; init; } = null!;

        [Required]
        public float Lon { get; init; }

        [Required]
        public float Lat { get; init; }
    }
}
