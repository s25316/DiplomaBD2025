using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.ValueObjects.BirthDates
{
    public class BirthDateException : TemplateException
    {
        public BirthDateException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
