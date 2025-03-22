// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.QueryParameters
{
    public class GeographyPointQueryParametersDto
    {
        // Properties
        public float? Lon { get; init; }

        public float? Lat { get; init; }


        // Methods
        public bool HasValue() => Lon.HasValue && Lat.HasValue;
    }
}
