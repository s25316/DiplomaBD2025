using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Requests.DTOs;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData.Request
{
    public class UserUpdatePersonDataCommand
    {
        public string? Description { get; init; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; init; } = null!;

        public string? ContactPhoneNumber { get; init; } = null!;

        public DateTime? BirthDate { get; init; }
        [Required]
        public bool IsTwoFactorAuthentication { get; set; }

        [Required]
        public bool IsStudent { get; set; }

        public AddressRequestDto? Address { get; init; }

        public List<int> SkillsIds { get; init; } = new List<int>();

        public IEnumerable<UrlRequestDto> Urls { get; init; } = [];
    }
}
