using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.ValueObjects.HoursPerTerms
{
    public class HoursPerTermException : TemplateException
    {
        public HoursPerTermException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
