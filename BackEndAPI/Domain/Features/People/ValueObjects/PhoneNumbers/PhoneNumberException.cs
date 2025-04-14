using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.ValueObjects.PhoneNumbers
{
    public class PhoneNumberException : TemplateException
    {
        public PhoneNumberException(string? message, HttpCode code = HttpCode.BadRequest)
            : base(message, code)
        {
        }
    }
}
