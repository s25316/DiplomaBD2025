using Domain.Features.Branches.ValueObjects;
using Domain.Features.ContractConditions.ValueObjects;
using Domain.Features.Offers.Enums;
using Domain.Features.Offers.ValueObjects;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Urls;

namespace Domain.Features.Offers.Entities
{
    public partial class Offer : TemplateEntity<OfferId>
    {
        // Properties
        public OfferTemplateId OfferTemplateId { get; private set; } = null!;
        public BranchId? BranchId { get; private set; } = null;
        public PublicationRange PublicationRange { get; private set; } = null!;
        public EmploymentLength? EmploymentLength { get; private set; }
        public UrlProperty? WebsiteUrl { get; private set; } = null;
        public IEnumerable<ContractConditionId> ContractConditionIds { get; private set; } = [];
        // Calculated Data
        public OfferStatus Status { get; private set; } = OfferStatus.Undefined;


        // Methods
        private void SetBranchId(Guid? branchId)
        {
            BranchId = branchId.HasValue ? branchId : null;
        }

        private void SetEmploymentLength(float? employmentLength)
        {
            EmploymentLength = employmentLength.HasValue ? employmentLength.Value : null;
        }

        private void SetContractConditionIds(IEnumerable<Guid> ids)
        {
            ContractConditionIds = ids.Select(x => (ContractConditionId)x).ToHashSet();
        }

        private void SetPublicationRange(DateTime start, DateTime? end)
        {
            var now = CustomTimeProvider.GetDateTimeNow();

            PublicationRange = new PublicationRange(start, end);

            if (PublicationRange.Start > now)
            {
                Status = OfferStatus.NotStarted;
            }
            else
            {
                Status = OfferStatus.Started;
            }
        }

        private void SetWebsiteUrl(string? websiteUrl)
        {
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? (UrlProperty?)null
                : websiteUrl.Trim();
        }
    }
}
