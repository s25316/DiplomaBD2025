// Ignore Spelling: regon krs

using Domain.Features.Companies.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.Companies.Entities
{
    public partial class Company : TemplateEntity<CompanyId>
    {
        public class Builder : TemplateBuilder<Company, CompanyId>
        {
            // Public Methods
            public Builder SetId(Guid companyId)
            {
                SetProperty(company => company.Id = (CompanyId)companyId);
                return this;
            }

            public Builder SetLogo(string? logo)
            {
                SetProperty(company => company.SetLogo(logo));
                return this;
            }

            public Builder SetName(string? name)
            {
                SetProperty(company => company.SetName(name));
                return this;
            }

            public Builder SetDescription(string? description)
            {
                SetProperty(company => company.SetDescription(description));
                return this;
            }

            public Builder SetRegon(string? regon)
            {
                SetProperty(company => company.SetRegon(regon));
                return this;
            }

            public Builder SetNip(string? nip)
            {
                SetProperty(company => company.SetNip(nip));
                return this;
            }

            public Builder SetKrs(string? krs)
            {
                SetProperty(company => company.SetKrs(krs));
                return this;
            }

            public Builder SetWebsiteUrl(string? websiteUrl)
            {
                SetProperty(company => company.SetWebsiteUrl(websiteUrl));
                return this;
            }

            public Builder SetCreated(DateTime created)
            {
                SetProperty(company => company.Created = created);
                return this;
            }

            public Builder SetRemoved(DateTime? removed)
            {
                SetProperty(company => company.Removed = removed);
                return this;
            }

            public Builder SetBlocked(DateTime? blocked)
            {
                SetProperty(company => company.Blocked = blocked);
                return this;
            }

            // Protected Methods
            protected override Action<Company> SetDefaultValues()
            {
                return company =>
                {
                    if (company.Created == DateTime.MinValue)
                    {
                        company.Created = CustomTimeProvider.Now;
                    }
                };
            }

            protected override Func<Company, string> CheckIsObjectComplete()
            {
                return company =>
                {
                    var builder = new StringBuilder();
                    if (string.IsNullOrWhiteSpace(company.Name))
                    {
                        builder.AppendLine(nameof(Company.Name));
                    }
                    if (company.Regon == null)
                    {
                        builder.AppendLine(nameof(Company.Regon));
                    }
                    if (company.Nip == null)
                    {
                        builder.AppendLine(nameof(Company.Nip));
                    }
                    return builder.ToString();
                };
            }
        }
    }
}
