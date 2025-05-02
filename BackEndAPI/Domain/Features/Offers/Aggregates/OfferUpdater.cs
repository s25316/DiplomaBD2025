using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.Offers.Aggregates
{
    public partial class Offer : TemplateEntity<OfferId>
    {
        public class Updater : TemplateUpdater<Offer, OfferId>
        {
            // Constructor
            public Updater(Offer value) : base(value) { }

            //Public Methods

            public Updater SetOfferTemplate(TemplateInfo template)
            {
                SetProperty(offer => offer.SetTemplate(template));
                return this;
            }

            public Updater SetBranchId(Guid? branchId)
            {
                SetProperty(offer => offer.SetBranchId(branchId));
                return this;
            }

            public Updater SetPublicationRange(
                DateTime start,
                DateTime? end)
            {
                SetProperty(offer => offer.SetPublicationRange(start, end));
                return this;
            }

            public Updater SetEmploymentLength(float? employmentLength)
            {
                SetProperty(offer => offer.SetEmploymentLength(employmentLength));
                return this;
            }

            public Updater SetWebsiteUrl(string? websiteUrl)
            {
                SetProperty(offer => offer.SetWebsiteUrl(websiteUrl));
                return this;
            }

            public Updater SetContractConditions(IEnumerable<ContractInfo> items)
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
