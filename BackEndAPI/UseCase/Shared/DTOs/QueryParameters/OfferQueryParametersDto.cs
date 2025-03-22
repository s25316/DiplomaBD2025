// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies.Offers;

namespace UseCase.Shared.DTOs.QueryParameters
{
    public class OfferQueryParametersDto
    {
        public OfferStatus? Status { get; init; }

        public DateTime? PublicationStartFrom { get; init; }

        public DateTime? PublicationStartTo { get; init; }

        public DateTime? PublicationEndFrom { get; init; }

        public DateTime? PublicationEndTo { get; init; }

        public float? EmploymentLengthFrom { get; init; }
        public float? EmploymentLengthTo { get; init; }
    }
}
