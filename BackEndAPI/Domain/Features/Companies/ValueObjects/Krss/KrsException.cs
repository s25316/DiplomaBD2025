// Ignore Spelling: Krs

using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.ValueObjects.Krss
{
    public class KrsException : TemplateException
    {
        public KrsException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
