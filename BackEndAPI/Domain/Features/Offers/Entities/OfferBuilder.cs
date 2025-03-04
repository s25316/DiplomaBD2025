using Domain.Features.Branches.ValueObjects;
using Domain.Features.Offers.ValueObjects;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using System.Text;

namespace Domain.Features.Offers.Entities
{
    public partial class Offer : TemplateEntity<OfferId>
    {
        public class Builder : TemplateBuilder<Offer>
        {
            //Public Methods
            public Builder SetId(Guid offerId)
            {
                SetProperty(offer => offer.Id = (OfferId)offerId);
                return this;
            }
            public Builder SetOfferTemplateId(Guid offerTemplateId)
            {
                SetProperty(offer =>
                    offer.OfferTemplateId = (OfferTemplateId)offerTemplateId
                );
                return this;
            }

            public Builder SetBranchId(Guid? branchId)
            {
                SetProperty(offer => offer.BranchId = (BranchId?)branchId);
                return this;
            }

            public Builder SetDatesRanges(
                DateTime publicationRangeStart,
                DateTime? publicationRangeEnd,
                DateOnly? workRangeStart,
                DateOnly? workRangeEnd)
            {
                SetProperty(offer => offer.SetDatesRanges(
                    publicationRangeStart,
                    publicationRangeEnd,
                    workRangeStart,
                    workRangeEnd));
                return this;
            }

            public Builder SetDatesRanges(
                DateTime publicationRangeStart,
                DateTime? publicationRangeEnd,
                DateTime? workRangeStart,
                DateTime? workRangeEnd)
            {
                var worStart = workRangeStart == null ?
                    (DateOnly?)null :
                    CustomTimeProvider.GetDateOnly(workRangeStart.Value);
                var worEnd = workRangeEnd == null ?
                    (DateOnly?)null :
                    CustomTimeProvider.GetDateOnly(workRangeEnd.Value);

                return SetDatesRanges(
                    publicationRangeStart,
                    publicationRangeEnd,
                    worStart,
                    worEnd);
            }

            public Builder SetSalaryData(
                decimal salaryRangeMin,
                decimal salaryRangeMax,
                int? salaryTermId,
                int? currencyId,
                bool isNegotiated)
            {
                SetProperty(offer => offer.SetSalaryData(
                    salaryRangeMin,
                    salaryRangeMax,
                    salaryTermId,
                    currencyId,
                    isNegotiated));
                return this;
            }

            public Builder SetWebsiteUrl(string? url)
            {
                SetProperty(offer => offer.SetWebsiteUrl(url));
                return this;
            }

            public Builder SetWorkModeIds(IEnumerable<int> workModeIds)
            {
                SetProperty(offer => offer.SetWorkModeIds(workModeIds));
                return this;
            }

            public Builder SetEmploymentTypeIds(IEnumerable<int> employmentTypeIds)
            {
                SetProperty(offer => offer.SetEmploymentTypeIds(employmentTypeIds));
                return this;
            }
            // Protected Methods 
            protected override Action<Offer> SetDefaultValues()
            {
                return offer => { };
            }

            protected override Func<Offer, string> CheckIsObjectComplete()
            {
                return offer =>
                {
                    var errors = new StringBuilder();
                    if (offer.OfferTemplateId == null)
                    {
                        errors.AppendLine($"{nameof(Offer.OfferTemplateId)}");
                    }
                    if (offer.PublicationRange == null)
                    {
                        errors.AppendLine($"about Publication");
                    }
                    if (offer.SalaryRange == null)
                    {
                        errors.AppendLine($"about Salary");
                    }
                    return errors.ToString();
                };
            }
        }
    }
}
