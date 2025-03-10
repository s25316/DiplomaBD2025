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
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches
{
    public class GetCompanyBranchesHandler : IRequestHandler<GetCompanyBranchesRequest, GetCompanyBranchesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

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
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(personId, query);

            var selectedValues = await query
                .Paginate(request.Page, request.ItemsPerPage)
                .Select(selector)
                .ToListAsync(cancellationToken);

            var totalCount = -1;
            var isForbidden = false;
            var isRemoved = false;
            var dtos = new List<CompanyAndBranchDto>();
            for (int i = 0; i < selectedValues.Count; i++)
            {
                var selectedValue = selectedValues[i];
                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                if (selectedValue.AuthorizedRolesCount == 0)
                {
                    isForbidden = true;
                    break;
                }
                if (selectedValue.Branch.Company.Removed != null)
                {
                    isRemoved = true;
                    break;
                }
                dtos.Add(new CompanyAndBranchDto
                {
                    Company = _mapper.Map<CompanyDto>(selectedValue.Branch.Company),
                    Branch = _mapper.Map<BranchDto>(selectedValue.Branch),
                });
            }

            if (!selectedValues.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }
            if (isForbidden)
            {
                return InvalidResponse(HttpCode.Forbidden);
            }
            if (isRemoved)
            {
                return InvalidResponse(HttpCode.Gone);
            }
            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }

        // Non Static Methods
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
                .AsNoTracking();
        }

        private IQueryable<Branch> BuildQuery(
            GetCompanyBranchesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();
            if (request.BranchId.HasValue)
            {
                var filter = BuildBranchFilter(request.BranchId.Value);
                query = query.Where(filter);
            }
            else
            {
                if (request.CompanyId.HasValue ||
                    request.Regon != null ||
                    request.Nip != null ||
                    request.Krs != null)
                {
                    var companyFilter = BuildCompanyFilters(
                       request.CompanyId,
                       request.Regon,
                       request.Nip,
                       request.Krs);
                    query = query.Where(companyFilter);
                }
                else
                {
                    query = query.Where(branch => branch.Company.CompanyPeople
                        .Any(role => _authorizedRoles.Any(roleId =>
                            role.RoleId == (int)roleId &&
                            role.PersonId == personId.Value &&
                            role.Deny == null
                        )));
                }

                var otherFilters = BuildOtherFilters(
                    request.SearchText,
                    request.ShowRemoved);
                query = query.Where(otherFilters);
                query = ApplyOrderBy(
                    query,
                    request.OrderBy,
                    request.Ascending,
                    request.ShowRemoved,
                    request.Lon,
                    request.Lat);
            }
            return query;
        }

        // Private static Methods
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
                        role.RoleId == (int)roleId &&
                        role.PersonId == personId.Value &&
                        role.Deny == null
                )),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static Expression<Func<Branch, bool>> BuildBranchFilter(
            Guid branchId)
        {
            return branch => branch.BranchId == branchId;
        }


        private static Expression<Func<Branch, bool>> BuildCompanyFilters(
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs)
        {
            if (companyId.HasValue)
            {
                return branch => branch.Company.CompanyId == companyId;
            }

            return branch =>
                (regon == null || branch.Company.Regon == regon) &&
                (nip == null || branch.Company.Nip == nip) &&
                (krs == null || branch.Company.Krs == krs);
        }

        private static Expression<Func<Branch, bool>> BuildOtherFilters(
            string? searchText,
            bool showRemoved)
        {
            var searchWords = CustomStringProvider.Split(searchText);

            return branch =>
                branch.Name != null &&
                branch.Company.Name != null &&
                (
                    !searchWords.Any() || searchWords.Any(word =>
                        branch.Name.Contains(word) ||
                        branch.Company.Name.Contains(word) ||
                        (branch.Description != null && branch.Description.Contains(word)) ||
                        (branch.Company.Description != null && branch.Company.Description.Contains(word))
                    )
                ) &&
                (
                     showRemoved
                        ? branch.Removed != null
                        : branch.Removed == null
                );
        }

        private static IQueryable<Branch> ApplyOrderBy(
            IQueryable<Branch> query,
            BranchesOrderBy orderBy,
            bool ascending,
            bool showRemoved,
            float? lon,
            float? lat
            )
        {
            if (orderBy == BranchesOrderBy.Point &&
                lon != null &&
                lat != null)
            {
                var point = new Point(lon.Value, lat.Value) { SRID = 4326 };
                return ascending ?
                    query.OrderBy(branch => branch.Address.Point.Distance(point))
                    .ThenBy(branch => branch.Created) :
                    query.OrderByDescending(branch =>
                        branch.Address.Point.Distance(point)
                        )
                    .ThenByDescending(branch => branch.Created);
            }
            if (orderBy == BranchesOrderBy.BranchRemoved &&
                showRemoved)
            {
                return ascending ?
                    query.OrderBy(branch => branch.Removed) :
                    query.OrderByDescending(branch => branch.Removed);
            }

            switch (orderBy)
            {
                case BranchesOrderBy.CompanyName:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Name) :
                        query.OrderByDescending(branch => branch.Company.Name);
                case BranchesOrderBy.CompanyCreated:
                    return ascending ?
                        query.OrderBy(branch => branch.Company.Created) :
                        query.OrderByDescending(branch => branch.Company.Created);
                case BranchesOrderBy.BranchName:
                    return ascending ?
                        query.OrderBy(branch => branch.Name) :
                        query.OrderByDescending(branch => branch.Name);
                default:
                    return ascending ?
                        query.OrderBy(branch => branch.Created) :
                        query.OrderByDescending(branch => branch.Created);
            }
        }

        private static GetCompanyBranchesResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyBranchesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndBranchDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                IsCorrect = false,
                HttpCode = code,
            };
        }

        private static GetCompanyBranchesResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyAndBranchDto> items,
            int totalCount)
        {
            return new GetCompanyBranchesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndBranchDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                IsCorrect = false,
                HttpCode = code,
            };
        }
    }
}
