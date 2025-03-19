using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.Exceptions
{
    public class MoneyException : TemplateException
    {
        public MoneyException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
