using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Response
{
    public class GetCompanyOffersResponse
    {
        public required IEnumerable<OfferDto> Offers { get; init; } = [];
    }
}
