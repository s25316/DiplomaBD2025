using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Recruitments.Exceptions
{
    public class RecruitmentException : TemplateException
    {
        // Constructor
        public RecruitmentException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
