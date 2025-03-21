// Ignore Spelling: Regon Krs, Dto

using UseCase.Shared.ValidationAttributes.CompanyAttributes;

namespace UseCase.Shared.DTOs.QueryParameters
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


        // Methods
        /// <summary>
        /// true if Regon, Nip, Krs not null
        /// </summary>
        /// <returns></returns>
        public bool ContainsAny()
        {
            return
                Regon != null ||
                Nip != null ||
                Krs != null;
        }
    }
}
