using Domain.Shared.ValueObjects.Guids;

namespace Domain.Shared.ValueObjects.Ids
{
    public record AddressId : GuidProperty
    {
        //Constructor
        public AddressId(Guid value) : base(value)
        {
        }

        // Public Methods
        public static implicit operator AddressId?(Guid? guid)
        {
            return guid.HasValue ? new AddressId(guid.Value) : null;
        }

        public static implicit operator Guid?(AddressId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator AddressId(Guid guid)
        {
            return new AddressId(guid);
        }

        public static implicit operator Guid(AddressId id)
        {
            return id.Value;
        }
    }
}
