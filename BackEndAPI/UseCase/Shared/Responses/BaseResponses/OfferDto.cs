// Ignore Spelling: Dto
using Domain.Features.Offers.Enums;
using System.Text.Json.Serialization;
using UseCase.Shared.JsonConverters;

namespace UseCase.Shared.Responses.BaseResponses
{
    public class OfferDto
    {
        public Guid OfferId { get; init; }

        public Guid? BranchId { get; set; }

        public DateTime PublicationStart { get; init; }

        public DateTime? PublicationEnd { get; init; }

        public float? EmploymentLength { get; init; }

        public string? WebsiteUrl { get; init; }

        public int StatusId { get; init; }

        [JsonConverter(typeof(OfferStatusJsonConverter))]
        public OfferStatus Status { get; init; }
    }
}
