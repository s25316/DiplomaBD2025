using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.ContractConditions.ValueObjects.Ids
{
    public record ContractAttributeId : GuidProperty
    {
        // Constructor
        public ContractAttributeId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator ContractAttributeId?(Guid? guid)
        {
            return guid.HasValue ? new ContractAttributeId(guid.Value) : null;
        }

        public static implicit operator Guid?(ContractAttributeId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator ContractAttributeId(Guid guid)
        {
            return new ContractAttributeId(guid);
        }

        public static implicit operator Guid(ContractAttributeId id)
        {
            return id.Value;
        }
    }
}
