using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserRecruitmentSetMessage.Request
{
    public class CompanyUserRecruitmentSetMessageCommand
    {
        [Required]
        public required string Message { get; init; }
    }
}
