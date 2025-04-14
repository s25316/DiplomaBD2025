namespace Domain.Features.OfferTemplates.ValueObjects.Info
{
    public record OfferSkillInfo
    {

        public required Guid? Id { get; init; }
        public required int SkillId { get; init; }
        public required bool IsRequired { get; init; }
        public required DateTime? Created { get; init; }
    }
}
