using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Offers.ValueObjects
{
    public record OfferId : GuidProperty
    {
        // Constructor
        public OfferId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator OfferId?(Guid? guid)
        {
            return guid.HasValue ? new OfferId(guid.Value) : null;
        }

        public static implicit operator Guid?(OfferId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator OfferId(Guid guid)
        {
            return new OfferId(guid);
        }

        public static implicit operator Guid(OfferId id)
        {
            return id.Value;
        }
    }
}
