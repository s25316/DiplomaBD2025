using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Branches
{
    public static class BranchEFExtensionMethods
    {
        public static IQueryable<Branch> SearchTextFilter(
            this IQueryable<Branch> query,
            IEnumerable<string> searchWords)
        {
            var expression = BranchEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }
    }
}
