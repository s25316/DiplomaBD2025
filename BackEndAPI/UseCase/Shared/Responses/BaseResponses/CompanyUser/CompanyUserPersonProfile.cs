using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser
{
    public class CompanyUserPersonProfile
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Description { get; set; }

        public string? PhoneNum { get; set; }

        public string? ContactEmail { get; set; }

        public int? Age { get; set; }

        public bool IsStudent { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Removed { get; set; }

        public IEnumerable<SkillDto> Skills { get; init; } = new List<SkillDto>();

        public IEnumerable<UrlDto> Urls { get; init; } = new List<UrlDto>();

        public virtual AddressResponseDto? Address { get; set; }
    }
}
