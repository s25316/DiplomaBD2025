using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request
{
    public class OfferTemplateUpdateCommand
    {
        public string? Name { get; init; } = null;

        public string? Description { get; init; } = null;

        public required IEnumerable<OfferSkillCreateDto> Skills { get; init; }
    }
}
