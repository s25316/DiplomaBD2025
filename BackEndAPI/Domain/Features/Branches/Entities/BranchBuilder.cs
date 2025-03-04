using Domain.Features.Branches.ValueObjects;
using Domain.Features.Companies.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects;
using System.Text;

namespace Domain.Features.Branches.Entities
{
    public partial class Branch : TemplateEntity<BranchId>
    {
        public class Builder : TemplateBuilder<Branch>
        {
            // Public Methods
            public Builder SetId(Guid branchId)
            {
                SetProperty(branch => branch.Id = (BranchId)branchId);
                return this;
            }

            public Builder SetCompanyId(Guid companyId)
            {
                SetProperty(branch => branch.CompanyId = (CompanyId)companyId);
                return this;
            }

            public Builder SetAddressId(Guid addressId)
            {
                SetProperty(branch => branch.AddressId = (AddressId)addressId);
                return this;
            }

            public Builder SetName(string? name)
            {
                SetProperty(branch => branch.SetName(name));
                return this;
            }

            public Builder SetDescription(string? description)
            {
                SetProperty(branch => branch.SetDescription(description));
                return this;
            }

            public Builder SetCreated(DateTime created)
            {
                SetProperty(branch => branch.Created = created);
                return this;
            }

            public Builder SetRemoved(DateTime? removed)
            {
                SetProperty(branch => branch.Removed = removed);
                return this;
            }

            // Protected Methods
            protected override Action<Branch> SetDefaultValues()
            {
                return branch =>
                {
                    if (branch.Created == DateTime.MinValue)
                    {
                        branch.Created = CustomTimeProvider.GetDateTimeNow();
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
