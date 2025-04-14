using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Ids;

namespace Domain.Features.People.Entities
{
    public class PersonSkill : TemplateEntity<PersonSkillId>
    {
        // Properties
        public required SkillId SkillId { get; init; } = null!;

        public required DateTime Created { get; init; }

        public DateTime? Removed { get; private set; } = null;


        // Methods
        public void Remove()
        {
            if (!Removed.HasValue)
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public static implicit operator PersonSkill(PersonSkillInfo item)
        {
            return new PersonSkill
            {
                Id = item.Id,
                SkillId = item.SkillId,
                Created = item.Created ?? CustomTimeProvider.Now,
                Removed = item.Removed,
            };
        }
    }
}
