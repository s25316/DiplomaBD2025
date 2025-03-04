using Domain.Features.Branches.ValueObjects;
using Domain.Features.Offers.Enums;
using Domain.Features.Offers.Exceptions;
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
        public BranchId? BranchId { get; private set; }
        public PublicationRange PublicationRange { get; private set; } = null!;
        public WorkRange? WorkRange { get; private set; } = null;
        public SalaryRange SalaryRange { get; private set; } = null!;
        public int? SalaryTermId { get; private set; }
        public int? CurrencyId { get; private set; }
        public bool IsNegotiated { get; private set; } = false;
        public UrlProperty? WebsiteUrl { get; private set; } = null;
        public HashSet<int> WorkModeIds { get; private set; } = [];
        public HashSet<int> EmploymentTypeIds { get; private set; } = [];
        // Calculated Data
        public OfferStatusEnum Status { get; private set; } = OfferStatusEnum.Undefined;
        public bool IsPaid { get; private set; } = false;


        // Methods
        private void SetDatesRanges(
                DateTime publicationRangeStart,
                DateTime? publicationRangeEnd,
                DateOnly? workRangeStart,
                DateOnly? workRangeEnd)
        {
            var now = CustomTimeProvider.GetDateTimeNow();
            // Publication Data 
            PublicationRange = new PublicationRange(
                publicationRangeStart,
                publicationRangeEnd);

            if (PublicationRange.Start < now &&
                PublicationRange.End.HasValue &&
                PublicationRange.End.Value < now)
            {
                Status = OfferStatusEnum.Expired;
            }
            else if (PublicationRange.Start > now)
            {
                Status = OfferStatusEnum.Future;
            }
            else
            {
                Status = OfferStatusEnum.Active;
            }

            // Work Range Data
            WorkRange = workRangeStart.HasValue ?
                new WorkRange(workRangeStart.Value, workRangeEnd) : null;

            if (WorkRange != null)
            {
                if (!PublicationRange.End.HasValue)
                {
                    throw new OfferException(Messages.Entity_Offer_EmptyPublicationEndForWorkRange);
                }
                if (
                    PublicationRange.End.HasValue &&
                    WorkRange.Start <= DateOnly.FromDateTime(PublicationRange.End.Value)
                    )
                {
                    throw new OfferException(Messages.Entity_Offer_WorkRangeLessPublicationEnd);
                }
            }
        }

        private void SetSalaryData(
                decimal salaryRangeMin,
                decimal salaryRangeMax,
                int? salaryTermId,
                int? currencyId,
                bool isNegotiated)
        {
            SalaryRange = new SalaryRange(salaryRangeMin, salaryRangeMax);
            if (SalaryRange.Min < 0)
            {
                throw new OfferException(Messages.Entity_Offer_SalaryRangeLessZero);
            }
            IsPaid = SalaryRange.Min > 0;

            if (IsPaid)
            {
                if (!salaryTermId.HasValue)
                {
                    throw new OfferException(Messages.Entity_Offer_SalaryTermIdRequired);
                }
                if (!currencyId.HasValue)
                {
                    throw new OfferException(Messages.Entity_Offer_CurrencyIdRequired);
                }
                SalaryTermId = salaryTermId;
                CurrencyId = currencyId;
                IsNegotiated = isNegotiated;
            }
        }

        private void SetWebsiteUrl(string? websiteUrl)
        {
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ?
                (UrlProperty?)null :
                websiteUrl.Trim();
        }

        private void SetWorkModeIds(IEnumerable<int> workModeIds)
        {
            WorkModeIds = new HashSet<int>(workModeIds);
        }

        private void SetEmploymentTypeIds(IEnumerable<int> employmentTypeIds)
        {
            EmploymentTypeIds = new HashSet<int>(employmentTypeIds);
        }
    }
}
