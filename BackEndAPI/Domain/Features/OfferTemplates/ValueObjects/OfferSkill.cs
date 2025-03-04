namespace Domain.Features.OfferTemplates.ValueObjects
{
    public record OfferSkill
    {
        public int SkillId { get; init; }
        public bool IsRequired { get; init; } = false;
    }
}
