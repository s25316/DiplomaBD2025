using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.Templates.Response.QueryResults;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers.Response
{
    public class GetCompanyUserOffersResponse : ResponseTemplate<ResponseQueryResultTemplate<OfferDto>>
    {
    }
}
