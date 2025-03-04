using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.OfferTemplates.ValueObjects
{
    public record OfferTemplateId : GuidProperty
    {
        // Constructor
        public OfferTemplateId(Guid value) : base(value)
        {
        }

        // Public Methods
        public static implicit operator OfferTemplateId?(Guid? guid)
        {
            return guid.HasValue ? new OfferTemplateId(guid.Value) : null;
        }

        public static implicit operator Guid?(OfferTemplateId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator OfferTemplateId(Guid guid)
        {
            return new OfferTemplateId(guid);
        }

        public static implicit operator Guid(OfferTemplateId id)
        {
            return id.Value;
        }
    }
}
