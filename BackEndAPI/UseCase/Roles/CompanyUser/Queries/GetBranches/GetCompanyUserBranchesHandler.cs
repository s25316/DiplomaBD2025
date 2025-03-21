using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches
{
    public class GetCompanyUserBranchesHandler : IRequestHandler<GetCompanyUserBranchesRequest, GetCompanyUserBranchesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        //Constructor
        public GetCompanyUserBranchesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyUserBranchesResponse> Handle(GetCompanyUserBranchesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(personId, query);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(selectedValues);
        }

        // Private Static Methods
        private sealed class SelectResult
        {
            public required Branch Branch { get; init; }
            public required int AuthorizedRolesCount { get; init; }
            public required int TotalCount { get; init; }

        }

        private static Expression<Func<Branch, SelectResult>> BuildSelector(
            PersonId personId,
            IQueryable<Branch> totalCountQuery)
        {
            return branch => new SelectResult
            {
                Branch = branch,
                AuthorizedRolesCount = branch.Company.CompanyPeople.Count(role =>
                    _authorizedRoles.Any(roleId =>
                        role.Deny == null &&
                        role.PersonId == personId.Value &&
                        role.RoleId == (int)roleId
                )),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static IQueryable<Branch> ApplyOrderBy(
            IQueryable<Branch> query,
            CompanyUserBranchesOrderBy orderBy,
            bool ascending,
            bool showRemoved,
            float? lon,
            float? lat)
        {
            if (orderBy == CompanyUserBranchesOrderBy.Point &&
                lon != null &&
                lat != null)
            {
                var point = new Point(lon.Value, lat.Value) { SRID = 4326 };
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

        private static GetCompanyUserBranchesResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserBranchesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndBranchDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserBranchesResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyAndBranchDto> items,
            int totalCount)
        {
            return new GetCompanyUserBranchesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndBranchDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(GetCompanyUserBranchesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
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
            var searchWords = CustomStringProvider.Split(request.SearchText);
            query = query.SearchTextFilter(searchWords);

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending,
                request.ShowRemoved,
                request.Lon,
                request.Lat);

            return query;
        }

        private GetCompanyUserBranchesResponse PrepareResponse(List<SelectResult> items)
        {
            if (!items.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<CompanyAndBranchDto>();
            for (int i = 0; i < items.Count; i++)
            {
                var selectedValue = items[i];

                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                Console.WriteLine(selectedValue.AuthorizedRolesCount);
                if (selectedValue.AuthorizedRolesCount == 0)
                {
                    return InvalidResponse(HttpCode.Forbidden);
                }
                if (selectedValue.Branch.Company.Removed != null)
                {
                    return InvalidResponse(HttpCode.Gone);
                }
                dtos.Add(new CompanyAndBranchDto
                {
                    Company = _mapper.Map<CompanyDto>(selectedValue.Branch.Company),
                    Branch = _mapper.Map<BranchDto>(selectedValue.Branch),
                });
            }
            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }
    }
}
