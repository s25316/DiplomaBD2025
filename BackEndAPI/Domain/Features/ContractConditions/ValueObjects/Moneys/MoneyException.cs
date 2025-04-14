using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.ValueObjects.Moneys
{
    public class MoneyException : TemplateException
    {
        public MoneyException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
