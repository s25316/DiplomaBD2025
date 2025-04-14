using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Shared.ValueObjects.Emails
{
    public class EmailException : TemplateException
    {
        public EmailException(string? message, HttpCode code = HttpCode.BadRequest)
            : base(message, code)
        {
        }
    }
}
