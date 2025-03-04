using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Offers.Exceptions
{
    public class OfferException : TemplateException
    {
        public OfferException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
