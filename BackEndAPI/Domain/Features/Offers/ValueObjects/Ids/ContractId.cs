using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Offers.ValueObjects.Ids
{
    public record ContractId : GuidProperty
    {
        // Constructor
        public ContractId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator ContractId?(Guid? guid)
        {
            return guid.HasValue ? new ContractId(guid.Value) : null;
        }

        public static implicit operator Guid?(ContractId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator ContractId(Guid guid)
        {
            return new ContractId(guid);
        }

        public static implicit operator Guid(ContractId id)
        {
            return id.Value;
        }
    }
}
