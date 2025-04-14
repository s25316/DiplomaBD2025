using Domain.Features.Branches.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using Domain.Shared.ValueObjects.Ids;
using System.Text;

namespace Domain.Features.Branches.Entities
{
    public partial class Branch : TemplateEntity<BranchId>
    {
        public class Updater : TemplateUpdater<Branch>
        {
            // Constructor
            public Updater(Branch value) : base(value)
            {
            }


            // Public Methods
            public Updater SetAddressId(Guid addressId)
            {
                SetProperty(branch => branch.AddressId = (AddressId)addressId);
                return this;
            }

            public Updater SetName(string? name)
            {
                SetProperty(branch => branch.SetName(name));
                return this;
            }

            public Updater SetDescription(string? description)
            {
                SetProperty(branch => branch.SetDescription(description));
                return this;
            }


            // Protected Methods
            protected override Action<Branch> SetDefaultValues()
            {
                return branch =>
                {
                    if (branch.Created == DateTime.MinValue)
                    {
                        branch.Created = CustomTimeProvider.Now;
                    }
                };
            }

            protected override Func<Branch, string> CheckIsObjectComplete()
            {
                return branch =>
                {
                    var builder = new StringBuilder();
                    if (branch.CompanyId == null)
                    {
                        builder.AppendLine(nameof(Branch.CompanyId));
                    }
                    if (branch.AddressId == null)
                    {
                        builder.AppendLine(nameof(Branch.AddressId));
                    }
                    if (string.IsNullOrWhiteSpace(branch.Name))
                    {
                        builder.AppendLine(nameof(Branch.Name));
                    }
                    return builder.ToString();
                };
            }
        }
    }
}
