using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer.Request
{
    public class UserRecruitsOfferCommand
    {
        [Required]
        public required IFormFile File { get; init; } = null!;

        public string? Description { get; init; }
    }
}
