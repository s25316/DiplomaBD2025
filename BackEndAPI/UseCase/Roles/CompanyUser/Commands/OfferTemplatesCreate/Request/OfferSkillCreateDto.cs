// Ignore Spelling: Dto

using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request
{
    public class OfferSkillCreateDto
    {
        [Required]
        public int SkillId { get; init; }

        [Required]
        public bool IsRequired { get; init; }
    }
}
