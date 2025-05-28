namespace UseCase.Shared.Responses.BaseResponses.Guest
{
    public class GuestOfferTemplateDto
    {
        public Guid OfferTemplateId { get; init; }

        public Guid CompanyId { get; init; }

        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;

        public DateTime Created { get; init; }

        public IEnumerable<OfferSkillDto> Skills { get; init; } = [];
    }
}
