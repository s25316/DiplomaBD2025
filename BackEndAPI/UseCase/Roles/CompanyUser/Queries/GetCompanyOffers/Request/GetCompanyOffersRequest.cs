using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Request
{
    public class GetCompanyOffersRequest : RequestTemplate<GetCompanyOffersResponse>
    {
        public required Guid CompanyId { get; init; }
        public required Guid OfferTemplateId { get; init; }
    }
}
