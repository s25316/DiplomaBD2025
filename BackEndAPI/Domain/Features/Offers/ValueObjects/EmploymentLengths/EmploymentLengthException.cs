using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Offers.ValueObjects.EmploymentLengths
{
    public class EmploymentLengthException : TemplateException
    {
        public EmploymentLengthException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
