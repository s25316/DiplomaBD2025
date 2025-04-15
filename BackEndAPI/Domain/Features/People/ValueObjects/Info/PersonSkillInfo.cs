using Domain.Shared.CustomProviders;

namespace Domain.Features.People.ValueObjects.Info
{
    public class PersonSkillInfo
    {
        // Properties
        public required Guid? Id { get; init; }

        public required int SkillId { get; init; }

        public required DateTime? Created { get; init; }

        public required DateTime? Removed { get; init; }


        // Methods
        public static implicit operator PersonSkillInfo(int skillId)
        {
            return new PersonSkillInfo
            {
                Id = null,
                SkillId = skillId,
                Created = CustomTimeProvider.Now,
                Removed = null,
            };
        }
    }
}
