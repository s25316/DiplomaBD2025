using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.Exceptions
{
    public class ContractConditionException : TemplateException
    {
        public ContractConditionException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
