using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.Exceptions
{
    public class CompanyException : TemplateException
    {
        public CompanyException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
