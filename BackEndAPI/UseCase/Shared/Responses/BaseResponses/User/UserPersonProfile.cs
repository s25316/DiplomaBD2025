using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.Responses.BaseResponses.User
{
    public class UserPersonProfile
    {
        public string? Logo { get; init; }

        public string? Name { get; init; }

        public string? Surname { get; init; }

        public string? Description { get; init; }

        public string? PhoneNum { get; init; }

        public string? ContactEmail { get; init; }

        public DateTime? BirthDate { get; init; }

        public bool IsTwoFactorAuth { get; init; }

        public bool IsStudent { get; init; }

        public bool IsAdmin { get; init; }

        public DateTime Created { get; init; }

        public DateTime? Blocked { get; init; }

        public DateTime? Removed { get; init; }

        public IEnumerable<SkillDto> Skills { get; init; } = new List<SkillDto>();

        public IEnumerable<UrlDto> Urls { get; init; } = new List<UrlDto>();

        public virtual AddressResponseDto? Address { get; init; }

    }
}
