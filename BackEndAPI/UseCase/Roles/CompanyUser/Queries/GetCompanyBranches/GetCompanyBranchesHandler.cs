using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches
{
    public class GetCompanyBranchesHandler : IRequestHandler<GetCompanyBranchesRequest, GetCompanyBranchesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<int> _autorizeRoles = [1];

        //Constructor
        public GetCompanyBranchesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyBranchesResponse> Handle(GetCompanyBranchesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var filters = BuildFilters(request);
            var baseQuery = BuildBaseQuery();
            Expression<Func<Branch, object>> selector = branch => new
            {
                Branch = branch,
                RolesCount = branch.Company.CompanyPeople.Count(role =>
                    _autorizeRoles.Any(roleId =>
                        role.RoleId == roleId &&
                        role.Deny == null &&
                        role.PersonId == personId.Value
                )),
                TotalCount = baseQuery.Where(filters).Count(),
            };

            var query = baseQuery
                .Where(filters)
                .AsQueryable();

            query = ApplyOrderBy(query, request);
            var branches = await query
                .Paginate(request.Page, request.ItemsPerPage)

                .ToListAsync(cancellationToken);

            return new GetCompanyBranchesResponse
            {
                Result = new GetCompanyBranchesQueryResult
                {
                    Items = branches.Select(branch => new CompanyAndBranchDto
                    {
                        Company = _mapper.Map<CompanyDto>(branch.Company),
                        Branch = _mapper.Map<BranchDto>(branch),
                    }),
                    TotalCount = 0,
                },
                IsCorrect = true,
                HttpCode = HttpCode.Ok,
            };
        }

        private PersonId GetPersonId(GetCompanyBranchesRequest request)
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
                .Where(branch =>
                    branch.Company.Name != null &&
                    branch.Company.Regon != null &&
                    branch.Company.Nip != null &&
                    branch.Company.Removed == null &&
                    branch.Name != null &&
                    branch.Address != null);
        }

        private static Expression<Func<Branch, bool>> BuildFiltersByBranchId(
            GetCompanyBranchesRequest request)
        {
            return branch => branch.BranchId == request.BranchId;
        }


        private static Expression<Func<Branch, bool>> BuildFiltersByCompany(
            GetCompanyBranchesRequest request)
        {
            if (request.CompanyId.HasValue)
            {
                return branch => branch.Company.CompanyId == request.CompanyId;
            }

            return branch =>
                (request.Regon == null || branch.Company.Regon == request.Regon) &&
                (request.Nip == null || branch.Company.Nip == request.Nip) &&
                (request.Krs == null || branch.Company.Krs == request.Krs);
        }

        private static Expression<Func<Branch, bool>> BuildFilters(GetCompanyBranchesRequest request)
        {
            char[] separators = { ' ', ',', '\n', '\t' };
            var searchWords = string.IsNullOrWhiteSpace(request.SearchText) ?
                [] :
                request.SearchText
                .Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return branch =>
                branch.Name != null &&
                branch.Company.Name != null &&
                (
                    !searchWords.Any() || searchWords.Any(word =>
                        branch.Name.Contains(word) ||
                        branch.Company.Name.Contains(word) ||
                        (
                            branch.Company.Description == null ||
                            branch.Company.Description.Contains(word)
                        ) ||
                        (
                            branch.Description == null ||
                            branch.Description.Contains(word)
                        ))
                ) &&
                (
                     request.ShowRemoved
                        ? branch.Removed != null
                        : branch.Removed == null
                );
        }

        private static IQueryable<Branch> ApplyOrderBy(
            IQueryable<Branch> query,
            GetCompanyBranchesRequest request)
        {
            if (
                request.OrderBy == BranchesOrderBy.Point &&
                request.Lon != null &&
                request.Lat != null
                )
            {
                var point = new Point(request.Lon.Value, request.Lat.Value) { SRID = 4326 };
                return request.Ascending ?
                    query.OrderBy(branch => branch.Address.Point.Distance(point))
                    .ThenBy(branch => branch.Created) :
                    query.OrderByDescending(branch =>
                        branch.Address.Point.Distance(point)
                        )
                    .ThenByDescending(branch => branch.Created);
            }
            if (
                !request.ShowRemoved &&
                request.OrderBy == BranchesOrderBy.BranchRemoved
                )
            {
                return request.Ascending ?
                            query.OrderBy(branch => branch.Removed) :
                            query.OrderByDescending(branch => branch.Removed);
            }

            switch (request.OrderBy)
            {
                case BranchesOrderBy.CompanyName:
                    return request.Ascending ?
                        query.OrderBy(branch => branch.Company.Name) :
                        query.OrderByDescending(branch => branch.Company.Name);
                case BranchesOrderBy.CompanyCreated:
                    return request.Ascending ?
                        query.OrderBy(branch => branch.Company.Created) :
                        query.OrderByDescending(branch => branch.Company.Created);
                case BranchesOrderBy.BranchName:
                    return request.Ascending ?
                        query.OrderBy(branch => branch.Name) :
                        query.OrderByDescending(branch => branch.Name);
                default:
                    return request.Ascending ?
                        query.OrderBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Created);
            }
        }
    }
}
