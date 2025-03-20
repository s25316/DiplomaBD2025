// Ignore Spelling: Regon Krs

using UseCase.Shared.ValidationAttributes.CompanyAttributes;

namespace BackEndAPI.QueryParameters
{
    public sealed class CompanyQueryParameters
    {
        [Regon]
        public string? Regon { get; init; }

        [Nip]
        public string? Nip { get; init; }

        [Krs]
        public string? Krs { get; init; }
    }
}
