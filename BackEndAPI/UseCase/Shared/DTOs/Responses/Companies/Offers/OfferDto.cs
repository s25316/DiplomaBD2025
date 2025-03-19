// Ignore Spelling: Dto

using System.Text.Json.Serialization;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.JsonConverters;

namespace UseCase.Shared.DTOs.Responses.Companies.Offers
{
    public class OfferDto
    {
        public Guid OfferId { get; init; }

        public DateTime PublicationStart { get; init; }

        public DateTime? PublicationEnd { get; init; }

        public float? EmploymentLength { get; init; }

        public string? WebsiteUrl { get; init; }

        [JsonConverter(typeof(OfferStatusJsonConverter))]
        public OfferStatus Status { get; init; }

        public required CompanyDto Company { get; init; }

        public BranchDto? Branch { get; init; }

        public required OfferTemplateDto OfferTemplate { get; init; }

        public IEnumerable<ContractConditionDto> ContractConditions { get; init; } = [];
    }
}
