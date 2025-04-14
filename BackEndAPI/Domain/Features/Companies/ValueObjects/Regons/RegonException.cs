// Ignore Spelling: Regon

using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.ValueObjects.Regons
{
    public class RegonException : TemplateException
    {
        public RegonException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
