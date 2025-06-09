using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Faqs
{
    public static class FaqEFExtensionMethods
    {
        public static IQueryable<Faq> ShowRemoved(
            this IQueryable<Faq> query,
            bool? showRemoved)
        {
            var expression = FaqEFExpressions.ShowRemovedExpression(showRemoved);
            return query.Where(expression);
        }
    }
}
