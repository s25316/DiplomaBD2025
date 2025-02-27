using System.ComponentModel.DataAnnotations;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetOffers.Response
{
    public class OfferSkillResponseDto
    {
        [Required]
        public SkillResponseDto Skill { get; set; } = null!;
        public bool IsRequired { get; set; }
    }
}
