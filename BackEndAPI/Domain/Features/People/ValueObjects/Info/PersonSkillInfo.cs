namespace Domain.Features.People.ValueObjects.Info
{
    public class PersonSkillInfo
    {
        public Guid? Id { get; init; }

        public int SkillId { get; init; }

        public DateTime? Created { get; init; }

        public DateTime? Removed { get; init; }
    }
}
