using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.OfferTemplates.Exceptions
{
    class OfferTemplateException : TemplateException
    {
        public OfferTemplateException(string? message, HttpCode code = HttpCode.BadRequest) : base(message, code)
        {
        }
    }
}
