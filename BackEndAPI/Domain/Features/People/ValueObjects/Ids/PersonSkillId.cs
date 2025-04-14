using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.People.ValueObjects.Ids
{
    public record PersonSkillId : GuidProperty
    {
        // Constructor
        public PersonSkillId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator PersonSkillId?(Guid? guid)
        {
            return guid.HasValue ? new PersonSkillId(guid.Value) : null;
        }

        public static implicit operator Guid?(PersonSkillId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator PersonSkillId(Guid guid)
        {
            return new PersonSkillId(guid);
        }

        public static implicit operator Guid(PersonSkillId id)
        {
            return id.Value;
        }
    }
}
