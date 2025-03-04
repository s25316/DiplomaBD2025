using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Shared.Exceptions
{
    public class EnumException : TemplateException
    {
        public EnumException(string? message, HttpCode code = HttpCode.InternalServerError) : base(message, code)
        {
        }
    }
}
