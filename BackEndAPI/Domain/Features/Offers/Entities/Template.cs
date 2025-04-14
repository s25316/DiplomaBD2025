using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.OfferTemplates.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;

namespace Domain.Features.Offers.Entities
{
    public class Template : TemplateEntity<TemplateId>
    {
        // Properties
        public required OfferTemplateId OfferTemplateId { get; init; }
        public required DateTime Created { get; init; }
        public required DateTime? Removed { get; set; }

        // Methods
        public void Remove()
        {
            if (!Removed.HasValue)
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public static implicit operator Template(TemplateInfo item)
        {
            return new Template
            {
                Id = item.Id,
                OfferTemplateId = item.OfferTemplateId,
                Created = item.Created ?? CustomTimeProvider.Now,
                Removed = item.Removed,
            };
        }
    }
}
