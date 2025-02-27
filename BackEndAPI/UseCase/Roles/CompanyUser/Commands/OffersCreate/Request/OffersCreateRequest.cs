using MediatR;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Request
{
    public class OffersCreateRequest : IRequest<OffersCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required Guid OfferTemplateId { get; init; }
        public required IEnumerable<OfferCreateCommand> Commands { get; init; } = [];
    }
}
