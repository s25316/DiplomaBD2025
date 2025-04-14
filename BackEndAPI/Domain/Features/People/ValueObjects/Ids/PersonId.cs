using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.People.ValueObjects.Ids
{
    public record PersonId : GuidProperty
    {
        // Constructor
        public PersonId(Guid original) : base(original)
        {
        }


        // Public Methods
        public static implicit operator PersonId?(Guid? guid)
        {
            return guid.HasValue ? new PersonId(guid.Value) : null;
        }

        public static implicit operator Guid?(PersonId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator PersonId(Guid guid)
        {
            return new PersonId(guid);
        }

        public static implicit operator Guid(PersonId id)
        {
            return id.Value;
        }
    }
}
