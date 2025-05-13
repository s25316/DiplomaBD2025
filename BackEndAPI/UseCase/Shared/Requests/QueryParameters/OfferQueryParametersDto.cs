// Ignore Spelling: Dto
namespace UseCase.Shared.Requests.QueryParameters
{
    public class OfferQueryParametersDto
    {
        public DateTime? PublicationStartFrom { get; init; }

        public DateTime? PublicationStartTo { get; init; }

        public DateTime? PublicationEndFrom { get; init; }

        public DateTime? PublicationEndTo { get; init; }

        public float? EmploymentLengthFrom { get; init; }
        public float? EmploymentLengthTo { get; init; }
    }
}
