using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.ContractConditions.ValueObjects.Ids
{
    public record ContractConditionId : GuidProperty
    {
        // Constructor
        public ContractConditionId(Guid value) : base(value)
        {
        }

        // Public Methods
        public static implicit operator ContractConditionId?(Guid? guid)
        {
            return guid.HasValue ? new ContractConditionId(guid.Value) : null;
        }

        public static implicit operator Guid?(ContractConditionId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator ContractConditionId(Guid guid)
        {
            return new ContractConditionId(guid);
        }

        public static implicit operator Guid(ContractConditionId id)
        {
            return id.Value;
        }
    }
}
