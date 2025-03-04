using Domain.Features.Branches.Exceptions;
using Domain.Features.Branches.ValueObjects;
using Domain.Features.Companies.ValueObjects;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects;

namespace Domain.Features.Branches.Entities
{
    public partial class Branch : TemplateEntity<BranchId>
    {
        // Properties
        public CompanyId CompanyId { get; private set; } = null!;
        public AddressId AddressId { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; } = null;
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; } = null;


        //Methods
        private void SetName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BranchException(Messages.Enitity_Branch_EmptyName);
            }
            Name = name;
        }

        private void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ?
                null :
                description.Trim();
        }
    }
}
