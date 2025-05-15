// Ignore Spelling: Regon Krs, Nip

using Domain.Shared.CustomProviders.StringProvider;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Companies
{
    public static class CompanyEFExtensionMethods
    {
        // Public Methods
        /// <summary>
        /// By REGON NIP KRS and CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static IQueryable<Company> WhereIdentificationData(
            this IQueryable<Company> query,
            Guid? companyId,
            CompanyQueryParametersDto companyQueryParameters)
        {
            var expression = CompanyEFExpressions.IdentificationExpression(
                companyId,
                companyQueryParameters.Regon,
                companyQueryParameters.Nip,
                companyQueryParameters.Krs);
            return query.Where(expression);
        }

        public static IQueryable<Company> WhereText(
            this IQueryable<Company> query,
            string? searchText)
        {
            var searchWords = CustomStringProvider.Split(searchText, WhiteSpace.All);
            var expression = CompanyEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }

        public static IQueryable<Company> OrderBy(
            this IQueryable<Company> query,
            CompanyOrderBy orderBy,
            bool ascending)
        {
            switch (orderBy)
            {
                case CompanyOrderBy.Name:
                    return ascending ?
                        query.OrderBy(company => company.Name) :
                        query.OrderByDescending(company => company.Name);
                default:
                    return ascending ?
                        query.OrderBy(company => company.Created) :
                        query.OrderByDescending(company => company.Created);
            }
        }
    }
}
