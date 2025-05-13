using Domain.Shared.CustomProviders.StringProvider;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.OfferTemplates
{
    public static class OfferTemplateEFExtensionMethods
    {
        public static IQueryable<OfferTemplate> WhereText(
            this IQueryable<OfferTemplate> query,
            string? searchText)
        {
            var searchWords = CustomStringProvider.Split(searchText, WhiteSpace.All);
            var expression = OfferTemplateEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }
    }
}
