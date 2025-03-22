using UseCase.RelationalDatabase.Models;

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
    }
}
