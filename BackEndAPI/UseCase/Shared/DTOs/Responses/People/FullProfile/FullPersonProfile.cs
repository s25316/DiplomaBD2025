using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Responses.BaseResponses;

namespace UseCase.Shared.DTOs.Responses.People.FullProfile
{
    public class FullPersonProfile
    {
        public string? Logo { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Description { get; set; }

        public string? PhoneNum { get; set; }

        public string? ContactEmail { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool IsTwoFactorAuth { get; set; }

        public bool IsStudent { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Removed { get; set; }

        public IEnumerable<SkillDto> Skills { get; init; } = new List<SkillDto>();

        public IEnumerable<UrlDto> Urls { get; init; } = new List<UrlDto>();

        public virtual AddressResponseDto? Address { get; set; }

    }
}
