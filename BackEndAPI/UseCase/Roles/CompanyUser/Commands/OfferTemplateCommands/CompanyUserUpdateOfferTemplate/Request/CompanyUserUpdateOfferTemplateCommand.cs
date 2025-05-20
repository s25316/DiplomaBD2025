using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserCreateOfferTemplates.Request;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate.Request
{
    public class CompanyUserUpdateOfferTemplateCommand
    {
        public string? Name { get; init; } = null;

        public string? Description { get; init; } = null;

        public required IEnumerable<OfferSkillCreateDto> Skills { get; init; }
    }
}
