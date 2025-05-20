using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate.Request
{
    public class OfferSkillUpdateDto
    {
        [Required]
        public int SkillId { get; init; }

        [Required]
        public bool IsRequired { get; init; }
    }
}
