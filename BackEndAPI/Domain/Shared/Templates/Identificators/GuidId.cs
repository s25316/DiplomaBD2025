// Ignore Spelling: Guid Giuid
using Domain.Shared.Exceptions;

namespace Domain.Shared.Templates.Identificators
{
    public record GuidId
    {
        // Properties
        public Guid Value { get; init; }

        // Constructor
        public GuidId(Guid value)
        {
            Value = value;
        }

        // Methods
        public static implicit operator GuidId(Guid id)
        {
            return new GuidId(id);
        }

        public static implicit operator Guid(GuidId id)
        {
            return id.Value;
        }

        public static implicit operator GuidId(string id)
        {
            if (
                string.IsNullOrWhiteSpace(id) ||
                !Guid.TryParse(id, out var guidId)
                )
            {
                throw new GuidIdException(id);
            }
            return new GuidId(guidId);
        }

        public static implicit operator string(GuidId id)
        {
            return id.Value.ToString();
        }


    }
}
