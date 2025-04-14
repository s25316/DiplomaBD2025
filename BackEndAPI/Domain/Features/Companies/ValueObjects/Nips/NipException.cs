using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.ValueObjects.Nips
{
    public class NipException : TemplateException
    {
        public NipException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
