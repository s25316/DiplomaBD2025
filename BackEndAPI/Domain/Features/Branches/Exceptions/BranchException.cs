using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Branches.Exceptions
{
    public class BranchException : TemplateException
    {
        public BranchException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
