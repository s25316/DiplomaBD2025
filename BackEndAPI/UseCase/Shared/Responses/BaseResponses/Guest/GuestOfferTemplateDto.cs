namespace UseCase.Shared.Responses.BaseResponses.Guest
{
    public class GuestOfferTemplateDto
    {
        public Guid OfferTemplateId { get; set; }

        public Guid CompanyId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime Created { get; set; }

        public IEnumerable<OfferSkillDto> Skills { get; init; } = [];
    }
}
