// Ignore Spelling: krs

using Domain.Features.Companies.ValueObjects;
using Domain.Shared.Templates;
using System.Text;

namespace Domain.Features.Companies.Entities
{
    public partial class Company : TemplateEntity<CompanyId>
    {
        public static Updater Update(Company company) => new Updater(company);
        public class Updater
        {
            //Properties
            private readonly Company _company;
            public readonly StringBuilder Errors = new StringBuilder();


            // Constructor
            public Updater(Company company)
            {
                _company = company;
            }

            //Methods ...
            public Company Build()
            {
                return _company;
            }
        }
    }
}
