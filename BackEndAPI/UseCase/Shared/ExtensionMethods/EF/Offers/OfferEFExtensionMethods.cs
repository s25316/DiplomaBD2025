using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Offers
{
    public static class OfferEFExtensionMethods
    {
        public static IQueryable<Offer> SearchTextFilter(
            this IQueryable<Offer> query,
            IEnumerable<string> searchWords)
        {
            var expression = OfferEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }

        public static IQueryable<Offer> OfferParametersFilter(
            this IQueryable<Offer> query,
            OfferQueryParametersDto offerParameters)
        {
            var expression = OfferEFExpressions.OfferParametersExpression(offerParameters);
            return query.Where(expression);
        }
    }
}
