using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.ValueObjects.SalaryRanges
{
    public class SalaryRangeException : TemplateException
    {
        public SalaryRangeException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
