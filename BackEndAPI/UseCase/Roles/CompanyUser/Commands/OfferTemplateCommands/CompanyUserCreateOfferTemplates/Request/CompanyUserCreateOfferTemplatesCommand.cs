namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserCreateOfferTemplates.Request
{
    public class CompanyUserCreateOfferTemplatesCommand
    {
        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;

        public required IEnumerable<OfferSkillCreateDto> Skills { get; init; }
    }
}
