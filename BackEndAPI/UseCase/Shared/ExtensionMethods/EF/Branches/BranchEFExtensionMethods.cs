using Domain.Shared.CustomProviders.StringProvider;
using NetTopologySuite.Geometries;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches;
using UseCase.Roles.Guests.Queries.GuestGetBranches;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Branches
{
    public static class BranchEFExtensionMethods
    {
        public static IQueryable<Branch> WhereText(
            this IQueryable<Branch> query,
            string? searchText)
        {
            var searchWords = CustomStringProvider.Split(searchText, WhiteSpace.All);
            var expression = BranchEFExpressions.SearchTextExpression(searchWords);
            return query.Where(expression);
        }

        public static IQueryable<Branch> OrderBy(
            this IQueryable<Branch> query,
            GeographyPointQueryParametersDto geographyPoint,
            bool showRemoved,
            CompanyUserBranchOrderBy orderBy,
            bool ascending)
        {
            if (orderBy == CompanyUserBranchOrderBy.Point &&
                geographyPoint.Lon.HasValue &&
                geographyPoint.Lat.HasValue)
            {
                var point = new Point(
                    geographyPoint.Lon.Value,
                    geographyPoint.Lat.Value)
                { SRID = 4326 };

                return ascending ?
                    query.OrderBy(branch => branch.Address.Point.Distance(point))
                        .ThenBy(branch => branch.Created) :
                    query.OrderByDescending(branch => branch.Address.Point.Distance(point))
                        .ThenByDescending(branch => branch.Created);
            }
            if (orderBy == CompanyUserBranchOrderBy.BranchRemoved &&
                showRemoved)
            {
                return ascending ?
                    query.OrderBy(branch => branch.Removed)
                        .ThenBy(branch => branch.Created) :
                    query.OrderByDescending(branch => branch.Removed)
                        .ThenByDescending(branch => branch.Created);
            }

            switch (orderBy)
            {
                case CompanyUserBranchOrderBy.CompanyName:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Name) :
                        query.OrderByDescending(branch => branch.Company.Name);
                case CompanyUserBranchOrderBy.CompanyCreated:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Created) :
                        query.OrderByDescending(branch => branch.Company.Created);
                case CompanyUserBranchOrderBy.BranchName:
                    return ascending ?
                        query.OrderBy(branch => branch.Name)
                            .ThenBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Name)
                            .ThenByDescending(branch => branch.Created);
                default:
                    return ascending ?
                        query.OrderBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Created);
            }
        }

        public static IQueryable<Branch> OrderBy(
            this IQueryable<Branch> query,
            GeographyPointQueryParametersDto geographyPoint,
            GuestBranchOrderBy orderBy,
            bool ascending)
        {
            if (orderBy == GuestBranchOrderBy.Point &&
                geographyPoint.Lon.HasValue &&
                geographyPoint.Lat.HasValue)
            {
                var point = new Point(
                    geographyPoint.Lon.Value,
                    geographyPoint.Lat.Value)
                { SRID = 4326 };

                return ascending ?
                    query.OrderBy(branch => branch.Address.Point.Distance(point))
                        .ThenBy(branch => branch.Created) :
                    query.OrderByDescending(branch => branch.Address.Point.Distance(point))
                        .ThenByDescending(branch => branch.Created);
            }

            switch (orderBy)
            {
                case GuestBranchOrderBy.CompanyName:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Name) :
                        query.OrderByDescending(branch => branch.Company.Name);
                case GuestBranchOrderBy.CompanyCreated:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Created) :
                        query.OrderByDescending(branch => branch.Company.Created);
                case GuestBranchOrderBy.BranchName:
                    return ascending ?
                        query.OrderBy(branch => branch.Name)
                            .ThenBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Name)
                            .ThenByDescending(branch => branch.Created);
                default:
                    return ascending ?
                        query.OrderBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Created);
            }
        }
    }
}
