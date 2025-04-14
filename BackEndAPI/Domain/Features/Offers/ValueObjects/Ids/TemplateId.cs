using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Offers.ValueObjects.Ids
{
    public record TemplateId : GuidProperty
    {
        // Constructor
        public TemplateId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator TemplateId?(Guid? guid)
        {
            return guid.HasValue ? new TemplateId(guid.Value) : null;
        }

        public static implicit operator Guid?(TemplateId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator TemplateId(Guid guid)
        {
            return new TemplateId(guid);
        }

        public static implicit operator Guid(TemplateId id)
        {
            return id.Value;
        }
    }
}
