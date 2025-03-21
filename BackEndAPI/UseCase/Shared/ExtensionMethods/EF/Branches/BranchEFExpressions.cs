using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Branches
{
    public static class BranchEFExpressions
    {
        public static Expression<Func<Branch, bool>> SearchTextExpression(
            IEnumerable<string> searchWords)
        {
            return branch =>
                !searchWords.Any() ||
                searchWords.Any(word =>
                    (branch.Name != null && branch.Name.Contains(word)) ||
                    (branch.Description != null && branch.Description.Contains(word)) ||
                    (branch.Company.Name != null && branch.Company.Name.Contains(word)) ||
                    (branch.Company.Description != null && branch.Company.Description.Contains(word))
                    );
        }
    }
}
