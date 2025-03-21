// Ignore Spelling: Regon Krs, Nip

using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Companies
{
    public static class CompanyEFExtensionMethods
    {
        /// <summary>
        /// By REGON NIP KRS and CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static IQueryable<Company> IdentificationFilter(
            this IQueryable<Company> query,
            Guid? companyId,
            CompanyQueryParametersDto company)
        {
            return query.IdentificationFilter(
                companyId,
                company.Regon,
                company.Nip,
                company.Krs);
        }

        /// <summary>
        /// By REGON NIP KRS and CompanyId
        /// </summary>
        /// <param name="query"></param>
        /// <param name="companyId"></param>
        /// <param name="regon"></param>
        /// <param name="nip"></param>
        /// <param name="krs"></param>
        /// <returns></returns>
        public static IQueryable<Company> IdentificationFilter(
            this IQueryable<Company> query,
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs)
        {
            var expression = CompanyEFExpressions.IdentificationExpression(
                companyId,
                regon,
                nip,
                krs);
            return query.Where(expression);
        }

        public static IQueryable<Company> SearchTextFilter(
            this IQueryable<Company> query,
            IEnumerable<string> searchWords)
        {
            var expression = CompanyEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }

    }
}
