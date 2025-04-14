using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.Exceptions
{
    public class PersonException : TemplateException
    {
        // Constructor
        public PersonException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
