// Ignore Spelling: Regon Krs, Dto

using UseCase.Shared.ValidationAttributes.CompanyAttributes;

namespace UseCase.Shared.Requests.QueryParameters
{
    public sealed class CompanyQueryParametersDto
    {
        // Properties
        [Regon]
        public string? Regon { get; init; }

        [Nip]
        public string? Nip { get; init; }

        [Krs]
        public string? Krs { get; init; }

        // Computed Properties
        public bool HasValue =>
            Regon != null ||
            Nip != null ||
            Krs != null;
    }
}
