// Ignore Spelling: Regon Krs, Nip

using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Companies
{
    public static class CompanyEFExpressions
    {
        public static Expression<Func<Company, bool>> IdentificationExpression(
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs)
        {
            if (companyId.HasValue)
            {
                return company => company.CompanyId == companyId.Value;
            }

            return company =>
                (regon == null || company.Regon == regon) &&
                (nip == null || company.Nip == nip) &&
                (krs == null || company.Krs == krs);
        }

        public static Expression<Func<Company, bool>> SearchTextExpression(
            IEnumerable<string> searchWords)
        {
            return company =>
                !searchWords.Any() ||
                searchWords.Any(word =>
                    (company.Name != null && company.Name.Contains(word)) ||
                    (company.Description != null && company.Description.Contains(word))
                );
        }
    }
}
