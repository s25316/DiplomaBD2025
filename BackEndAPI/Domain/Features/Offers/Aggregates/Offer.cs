using Domain.Features.Branches.ValueObjects;
using Domain.Features.ContractConditions.ValueObjects.Ids;
using Domain.Features.Offers.Entities;
using Domain.Features.Offers.Exceptions;
using Domain.Features.Offers.ValueObjects;
using Domain.Features.Offers.ValueObjects.EmploymentLengths;
using Domain.Features.Offers.ValueObjects.Enums;
using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.OfferTemplates.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Urls;

namespace Domain.Features.Offers.Aggregates
{
    public partial class Offer : TemplateEntity<OfferId>
    {
        // Properties
        public BranchId? BranchId { get; private set; } = null;
        public PublicationRange PublicationRange { get; private set; } = null!;
        public EmploymentLength? EmploymentLength { get; private set; }
        public UrlProperty? WebsiteUrl { get; private set; } = null;
        // Collections
        private Dictionary<OfferTemplateId, Template> _templates = [];
        public IReadOnlyDictionary<OfferTemplateId, Template> Templates => _templates;

        private Dictionary<ContractConditionId, Contract> _contracts = [];
        public IReadOnlyDictionary<ContractConditionId, Contract> Contracts => _contracts;
        // Calculated Data
        public OfferStatus Status
        {
            get
            {
                var now = CustomTimeProvider.Now;
                if (PublicationRange == null)
                {
                    return OfferStatus.Undefined;
                }
                else if (PublicationRange.Start > now)
                {
                    return OfferStatus.Scheduled;
                }
                else if (PublicationRange.End != null && PublicationRange.End < now)
                {
                    return OfferStatus.Expired;
                }
                else
                {
                    return OfferStatus.Active;
                }
            }
        }


        // Public Methods
        /// <exception cref="OfferException"></exception>
        public void Remove()
        {
            if (Status == OfferStatus.Expired)
            {
                throw new OfferException(Messages.Entity_Offer_UnableRemoveExpired);
            }

            var now = CustomTimeProvider.Now;
            if (Status == OfferStatus.Active)
            {
                PublicationRange = new PublicationRange(
                    PublicationRange.Start,
                    now);
            }
            else
            {
                PublicationRange = new PublicationRange(
                    now,
                    now);
            }
        }

        // Private Methods
        private void SetBranchId(Guid? branchId)
        {
            BranchId = branchId.HasValue ? branchId : null;
        }

        private void SetEmploymentLength(float? employmentLength)
        {
            EmploymentLength = employmentLength.HasValue
                ? employmentLength.Value
                : null;
        }

        private void SetPublicationRange(DateTime start, DateTime? end)
        {
            if (PublicationRange == null)
            {
                PublicationRange = new PublicationRange(start, end);
            }
            else
            {
                switch (Status)
                {
                    case OfferStatus.Scheduled:
                        var privious = PublicationRange;

                        PublicationRange = new PublicationRange(start, end);
                        if (Status != OfferStatus.Scheduled)
                        {
                            PublicationRange = privious;
                            throw new OfferException(
                            "Invalid publication range data");
                        }
                        break;

                    case OfferStatus.Active:
                        var now = CustomTimeProvider.Now;
                        if (end == null || end > now)
                        {
                            PublicationRange = new PublicationRange(
                                PublicationRange.Start,
                                end);
                        }
                        else
                        {
                            throw new OfferException(
                            "Invalid publication End data");
                        }
                        break;

                    case OfferStatus.Expired:
                        throw new OfferException(
                            $"Offer {OfferStatus.Expired.Description()}");
                }
            }
        }

        private void SetWebsiteUrl(string? websiteUrl)
        {
            WebsiteUrl = !string.IsNullOrWhiteSpace(websiteUrl)
                ? new UrlProperty(websiteUrl.Trim())
                : null;
        }

        private void SetTemplate(TemplateInfo item)
        {
            if (_templates.ContainsKey(item.OfferTemplateId))
            {
                var template = (Template)item;
                _templates[template.OfferTemplateId] = template;
            }
            else
            {
                foreach (var value in _templates.Values)
                {
                    value.Remove();
                }
                var template = (Template)item;
                _templates[template.OfferTemplateId] = template;
            }
        }

        private void SetContractInfo(IEnumerable<ContractInfo> items)
        {
            var itemsDictionary = items
                .Select(i => (Contract)i)
                .ToDictionary(i => i.ContractConditionId);

            var existingKeys = _contracts.Keys.ToHashSet();
            var itemsKeys = itemsDictionary.Keys.ToHashSet();

            var intersectKeys = existingKeys.Intersect(itemsKeys);
            var removedKeys = existingKeys.Except(intersectKeys);
            var newKeys = itemsKeys.Except(intersectKeys);

            foreach (var key in removedKeys)
            {
                _contracts[key].Remove();
            }
            foreach (var key in newKeys)
            {
                _contracts[key] = itemsDictionary[key];
            }
        }
    }
}
