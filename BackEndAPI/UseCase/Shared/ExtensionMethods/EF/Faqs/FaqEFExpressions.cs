using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Faqs
{
    public static class FaqEFExpressions
    {
        public static Expression<Func<Faq, bool>> ShowRemovedExpression(bool? showRemoved)
        {
            return faq =>
                !showRemoved.HasValue ||
                (
                    (
                        showRemoved == true &&
                        faq.Removed != null
                    ) ||
                    (
                        showRemoved == false &&
                        faq.Removed == null
                    )
                );
        }
    }
}
