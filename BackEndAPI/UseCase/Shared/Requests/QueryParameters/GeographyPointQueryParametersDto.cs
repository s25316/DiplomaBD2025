// Ignore Spelling: Dto

namespace UseCase.Shared.Requests.QueryParameters
{
    public class GeographyPointQueryParametersDto
    {
        // Properties
        public float? Lon { get; init; }

        public float? Lat { get; init; }

        // Computed Properties
        public bool HasValue => Lon.HasValue && Lat.HasValue;
    }
}
