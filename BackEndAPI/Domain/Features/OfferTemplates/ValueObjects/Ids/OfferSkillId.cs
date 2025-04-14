using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.OfferTemplates.ValueObjects.Ids
{
    public record OfferSkillId : GuidProperty
    {
        // Constructor
        public OfferSkillId(Guid value) : base(value)
        {
        }

        // Public Methods
        public static implicit operator OfferSkillId?(Guid? guid)
        {
            return guid.HasValue ? new OfferSkillId(guid.Value) : null;
        }

        public static implicit operator Guid?(OfferSkillId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator OfferSkillId(Guid guid)
        {
            return new OfferSkillId(guid);
        }

        public static implicit operator Guid(OfferSkillId id)
        {
            return id.Value;
        }
    }
}
