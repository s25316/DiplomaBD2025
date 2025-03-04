// Ignore Spelling: Guid Giuid
namespace Domain.Shared.ValueObjects.Guids
{
    public record GuidProperty
    {
        // Properties
        public Guid Value { get; init; }

        // Constructor
        public GuidProperty(Guid value)
        {
            Value = value;
        }

        // Public Methods
        public static implicit operator GuidProperty?(Guid? guid)
        {
            return guid.HasValue ? new GuidProperty(guid.Value) : null;
        }

        public static implicit operator Guid?(GuidProperty? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator GuidProperty(Guid guid)
        {
            return new GuidProperty(guid);
        }

        public static implicit operator Guid(GuidProperty id)
        {
            return id.Value;
        }
    }
}
