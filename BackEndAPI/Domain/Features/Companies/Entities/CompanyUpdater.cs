// Ignore Spelling: krs

using Domain.Features.Companies.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.Companies.Entities
{
    public partial class Company : TemplateEntity<CompanyId>
    {
        public static Updater Update(Company company) => new Updater(company);
        public class Updater : TemplateUpdater<Company, CompanyId>
        {
            // Constructor
            public Updater(Company company) : base(company)
            {
            }


            // Public Methods
            public Updater SetDescription(string? description)
            {
                SetProperty(company => company.SetDescription(description));
                return this;
            }

            public Updater SetKrs(string? krs)
            {
                SetProperty(company => company.SetKrs(krs));
                return this;
            }

            public Updater SetWebsiteUrl(string? websiteUrl)
            {
                SetProperty(company => company.SetWebsiteUrl(websiteUrl));
                return this;
            }

            public Updater SetLogo(string? logo)
            {
                SetProperty(company => company.SetLogo(logo));
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
