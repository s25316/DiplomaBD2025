namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request
{
    public class OfferTemplateCreateCommand
    {
        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;

        public required IEnumerable<OfferSkillCreateDto> Skills { get; init; }
    }
}
