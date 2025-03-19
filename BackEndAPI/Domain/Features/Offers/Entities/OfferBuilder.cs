using Domain.Features.Offers.ValueObjects;
using Domain.Shared.Templates;
using System.Text;

namespace Domain.Features.Offers.Entities
{
    public partial class Offer : TemplateEntity<OfferId>
    {
        public class Builder : TemplateBuilder<Offer>
        {
            //Public Methods
            public Builder SetId(Guid id)
            {
                SetProperty(offer => offer.Id = id);
                return this;
            }

            public Builder SetOfferTemplateId(Guid offerTemplateId)
            {
                SetProperty(offer => offer.OfferTemplateId = offerTemplateId);
                return this;
            }

            public Builder SetBranchId(Guid? branchId)
            {
                SetProperty(offer => offer.SetBranchId(branchId));
                return this;
            }

            public Builder SetPublicationRange(
                DateTime start,
                DateTime? end)
            {
                SetProperty(offer => offer.SetPublicationRange(start, end));
                return this;
            }

            public Builder SetEmploymentLength(float? employmentLength)
            {
                SetProperty(offer => offer.SetEmploymentLength(employmentLength));
                return this;
            }

            public Builder SetWebsiteUrl(string? websiteUrl)
            {
                SetProperty(offer => offer.SetWebsiteUrl(websiteUrl));
                return this;
            }

            public Builder SetContractConditionIds(IEnumerable<Guid> ids)
            {
                SetProperty(offer => offer.SetContractConditionIds(ids));
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
                    return errors.ToString();
                };
            }
        }
    }
}
