using Domain.Features.OfferTemplates.ValueObjects.Ids;
using Domain.Features.OfferTemplates.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Ids;

namespace Domain.Features.OfferTemplates.Entities
{
    public class OfferSkill : TemplateEntity<OfferSkillId>
    {
        // Properties
        public SkillId SkillId { get; init; } = null!;
        public DateTime Created { get; init; }
        public bool IsRequired { get; set; } = false;
        public DateTime? Removed { get; private set; } = null;


        // Methods

        public void Remove()
        {
            if (!Removed.HasValue)
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public static implicit operator OfferSkill(OfferSkillInfo item)
        {
            return new OfferSkill
            {
                Id = item.Id,
                SkillId = item.SkillId,
                Created = item.Created ?? CustomTimeProvider.Now,
                IsRequired = item.IsRequired
            };
        }
    }
}
