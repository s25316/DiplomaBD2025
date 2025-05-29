using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitmentSetMessage.Request
{
    public class UserRecruitmentSetMessageCommand
    {
        [Required]
        public required string Message { get; init; }
    }
}
