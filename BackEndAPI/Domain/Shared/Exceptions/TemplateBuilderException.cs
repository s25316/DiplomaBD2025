using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Shared.Exceptions
{
    public class TemplateBuilderException : TemplateException
    {
        public TemplateBuilderException(
            string? message,
            HttpCode code = HttpCode.InternalServerError) : base(message, code)
        {
        }
    }
}
