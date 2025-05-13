using Domain.Features.Offers.Enums;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Offers
{
    public static class OfferEFExtensionMethods
    {
        public static IQueryable<Offer> WhereOfferParameters(
            this IQueryable<Offer> query,
            OfferQueryParametersDto offerParameters)
        {
            var expression = OfferEFExpressions.OfferParametersExpression(offerParameters);
            return query.Where(expression);
        }

        public static IQueryable<Offer> WhereStatus(
            this IQueryable<Offer> query,
            OfferStatus? status)
        {
            var expression = OfferEFExpressions.OfferStatusExpression(status);
            return query.Where(expression);
        }
    }
}
