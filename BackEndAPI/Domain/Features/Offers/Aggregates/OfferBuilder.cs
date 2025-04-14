using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.Offers.Aggregates
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

            public Builder SetOfferTemplate(TemplateInfo template)
            {
                SetProperty(offer => offer.SetTemplate(template));
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

            public Builder SetContractConditions(IEnumerable<ContractInfo> items)
            {
                SetProperty(offer => offer.SetContractInfo(items));
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
                    var coreTemplate = offer.Templates.Values
                        .FirstOrDefault(t => t.Removed == null);
                    if (coreTemplate == null)
                    {
                        errors.AppendLine($"Not existing Template");
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
