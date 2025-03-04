using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Shared.ValueObjects.Urls
{
    public class UrlPropertyException : TemplateException
    {
        public UrlPropertyException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
