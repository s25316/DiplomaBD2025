namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request
{
    public record OfferTemplateCommand
    {
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
        public IEnumerable<OfferSkillRequestDto> Skills { get; init; } = [];
    }
}
