using Domain.Features.Companies.ValueObjects;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using System.Text;

namespace Domain.Features.OfferTemplates.Entities
{
    public partial class OfferTemplate : TemplateEntity<OfferTemplateId>
    {
        public class Builder : TemplateBuilder<OfferTemplate>
        {
            // Public Methods
            public Builder SetId(Guid offerTemplateId)
            {
                SetProperty(template => template.Id = (OfferTemplateId)offerTemplateId);
                return this;
            }

            public Builder SetCompanyId(Guid companyId)
            {
                SetProperty(template => template.CompanyId = (CompanyId)companyId);
                return this;
            }

            public Builder SetName(string name)
            {
                SetProperty(template => template.SetName(name));
                return this;
            }

            public Builder SetDescription(string description)
            {
                SetProperty(template => template.SetDescription(description));
                return this;
            }

            public Builder SetCreated(DateTime created)
            {
                SetProperty(template => template.Created = created);
                return this;
            }

            public Builder SetRemoved(DateTime? removed)
            {
                SetProperty(template => template.Removed = removed);
                return this;
            }

            public Builder SetSkillsIds(IEnumerable<OfferSkill> skills)
            {
                SetProperty(template => template.SetSkillsIds(skills));
                return this;
            }

            // Protected Methods
            protected override Action<OfferTemplate> SetDefaultValues()
            {
                return template =>
                {
                    if (template.Created == DateTime.MinValue)
                    {
                        template.Created = CustomTimeProvider.GetDateTimeNow();
                    }
                };
            }

            protected override Func<OfferTemplate, string> CheckIsObjectComplete()
            {
                return template =>
                {
                    var builder = new StringBuilder();
                    if (template.CompanyId == null)
                    {
                        builder.AppendLine(nameof(OfferTemplate.CompanyId));
                    }
                    if (string.IsNullOrWhiteSpace(template.Name))
                    {
                        builder.AppendLine(nameof(OfferTemplate.Name));
                    }
                    if (string.IsNullOrWhiteSpace(template.Description))
                    {
                        builder.AppendLine(nameof(OfferTemplate.Description));
                    }
                    return builder.ToString();
                };
            }
        }
    }
}
