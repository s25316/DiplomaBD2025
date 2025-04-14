using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders.StringProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Response;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.QueryParameters;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches
{
    public class GetCompanyUserBranchesHandler :
        GetCompanyUserGenericsBase<Branch, CompanyAndBranchDto>,
        IRequestHandler<GetCompanyUserBranchesRequest, GetCompanyUserGenericItemsResponse<CompanyAndBranchDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        //Constructor
        public GetCompanyUserBranchesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
            : base(context, mapper, authenticationInspector)
        { }


        // Methods
        public async Task<GetCompanyUserGenericItemsResponse<CompanyAndBranchDto>> Handle(GetCompanyUserBranchesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                branch => branch.Company.CompanyPeople);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectedValues,
                branch => branch.Company.Removed != null,
                branch => new CompanyAndBranchDto
                {
                    Company = _mapper.Map<CompanyDto>(branch.Company),
                    Branch = _mapper.Map<BranchDto>(branch),
                });
        }

        private static IQueryable<Branch> ApplyOrderBy(
            IQueryable<Branch> query,
            CompanyUserBranchesOrderBy orderBy,
            bool ascending,
            bool showRemoved,
            GeographyPointQueryParametersDto geographyPoint)
        {
            if (orderBy == CompanyUserBranchesOrderBy.Point &&
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
            if (orderBy == CompanyUserBranchesOrderBy.BranchRemoved &&
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
                case CompanyUserBranchesOrderBy.CompanyName:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Name) :
                        query.OrderByDescending(branch => branch.Company.Name);
                case CompanyUserBranchesOrderBy.CompanyCreated:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Created) :
                        query.OrderByDescending(branch => branch.Company.Created);
                case CompanyUserBranchesOrderBy.BranchName:
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

        private IQueryable<Branch> BuildBaseQuery()
        {
            return _context.Branches
                .Include(b => b.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(b => b.Address)
                .ThenInclude(a => a.Street)

                .Include(b => b.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(a => a.State)
                .ThenInclude(a => a.Country)
                .AsNoTracking();
        }

        private IQueryable<Branch> BuildQuery(
            GetCompanyUserBranchesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();

            // Choose only One Branch
            if (request.BranchId.HasValue)
            {
                return query.Where(branch => branch.BranchId == request.BranchId.Value);
            }

            // Choose Branches by Company even we haven`t access 
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                query = query.Where(branch => _context.Companies
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters)
                    .Any(company => company.CompanyId == branch.CompanyId)
                );
            }
            // Choose Branches where we have access and eliminate Removed Companies 
            else
            {
                query = query
                    .Where(branch => branch.Company.Removed == null)
                    .Where(branch => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(cp => cp.CompanyId == branch.CompanyId));
            }

            // Select Removed or not removed
            query = query.Where(branch => request.ShowRemoved
                        ? branch.Removed != null
                        : branch.Removed == null);
            // Search Text
            var searchWords = CustomStringProvider
                .Split(request.SearchText, WhiteSpace.All);

            query = query.SearchTextFilter(searchWords);

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending,
                request.ShowRemoved,
                request.GeographyPoint);

            return query;
        }
    }
}
