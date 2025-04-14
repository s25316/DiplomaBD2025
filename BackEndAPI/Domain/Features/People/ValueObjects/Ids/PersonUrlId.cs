using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.People.ValueObjects.Ids
{
    public record PersonUrlId : GuidProperty
    {
        // Constructor
        public PersonUrlId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator PersonUrlId?(Guid? guid)
        {
            return guid.HasValue ? new PersonUrlId(guid.Value) : null;
        }

        public static implicit operator Guid?(PersonUrlId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator PersonUrlId(Guid guid)
        {
            return new PersonUrlId(guid);
        }

        public static implicit operator Guid(PersonUrlId id)
        {
            return id.Value;
        }
    }
}
