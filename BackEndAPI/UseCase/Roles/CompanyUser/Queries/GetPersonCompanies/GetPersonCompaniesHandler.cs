using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies
{
    public class GetPersonCompaniesHandler : IRequestHandler<GetPersonCompaniesRequest, GetPersonCompaniesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<int> _authorizationRoles = [1];


        //Constructor
        public GetPersonCompaniesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetPersonCompaniesResponse> Handle(GetPersonCompaniesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var baseQuery = _context.Companies
                    .Include(c => c.CompanyPeople)
                    .AsNoTracking()
                    .AsQueryable();

            // Parameters Defines The Company
            if (
                request.CompanyId != null ||
                request.Regon != null ||
                request.Nip != null ||
                request.Krs != null
                )
            {
                return await HandleSingleCompanyAsync(
                    request,
                    personId,
                    baseQuery,
                    cancellationToken);
            }
            else
            {
                return await HandleListCompaniesAsync(
                    request,
                    personId,
                    baseQuery,
                    cancellationToken);
            }
        }

        // Private Methods
        private PersonId GetPersonId(GetPersonCompaniesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private async Task<GetPersonCompaniesResponse> HandleSingleCompanyAsync(
            GetPersonCompaniesRequest request,
            PersonId personId,
            IQueryable<Company> baseQuery,
            CancellationToken cancellationToken)
        {
            var filters = BuildFiltersForSingleCompany(request);
            var query = baseQuery.Where(filters);

            var singleCompany = await query.Select(company => new
            {
                Company = company,
                CountRoleIds = company.CompanyPeople
                    .Count(role => _authorizationRoles.Any(id =>
                        role.RoleId == id &&
                        role.Deny == null &&
                        role.PersonId == personId.Value
                    ))
            })
            .FirstOrDefaultAsync(cancellationToken);

            switch (singleCompany)
            {
                case null:
                    return InvalidResponse(HttpCode.NotFound);
                case { CountRoleIds: 0 }:
                    return InvalidResponse(HttpCode.Forbidden);
                default:
                    var item = new GetPersonCompaniesQueryResult
                    {
                        Items = [_mapper.Map<CompanyDto>(singleCompany.Company)],
                        TotalCount = 1,
                    };
                    return ValidResponse(HttpCode.Ok, item);
            }
        }

        private async Task<GetPersonCompaniesResponse> HandleListCompaniesAsync(
           GetPersonCompaniesRequest request,
           PersonId personId,
           IQueryable<Company> baseQuery,
           CancellationToken cancellationToken)
        {
            var filters = BuildFiltersForListCompanies(personId, request);
            var query = baseQuery.Where(filters);
            var notPaginatedQuery = query;

            query = ApplyOrderBy(query, request);
            query = query.Paginate(request.Page, request.ItemsPerPage);

            var companies = await query
                .Select(company => new
                {
                    Company = company,
                    TotalCount = notPaginatedQuery.Count(),
                })
                .ToListAsync(cancellationToken);

            if (!companies.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var result = new GetPersonCompaniesQueryResult
            {
                Items = companies
                        .Select(x => _mapper.Map<CompanyDto>(x.Company)),
                TotalCount = companies[0].TotalCount,
            };
            return ValidResponse(HttpCode.Ok, result);
        }

        // Private Static Methods
        private static Expression<Func<Company, bool>> BuildFiltersForSingleCompany(
            GetPersonCompaniesRequest request)
        {
            if (request.CompanyId != null)
            {
                return company => company.CompanyId == request.CompanyId;
            }
            return company =>
                    (request.Regon == null || company.Regon == request.Regon) &&
                    (request.Nip == null || company.Nip == request.Nip) &&
                    (request.Krs == null || company.Krs == request.Krs);
        }

        private static Expression<Func<Company, bool>> BuildFiltersForListCompanies(
            PersonId personId,
            GetPersonCompaniesRequest request)
        {
            char[] separators = { ' ', ',', '\n', '\t' };
            var searchWords = string.IsNullOrWhiteSpace(request.SearchText)
                ? []
                : request.SearchText
                .Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Expression<Func<Company, bool>> filters = company =>
                company.Name != null &&
                company.Nip != null &&
                company.Regon != null &&
                company.Removed == null &&

                _authorizationRoles.Any(roleId => company.CompanyPeople.Any(cp =>
                    cp.Deny == null &&
                    cp.PersonId == personId.Value &&
                    cp.RoleId == roleId
                    ))
                 &&
                (
                    !searchWords.Any() ||
                    searchWords.Any(word =>
                        company.Name.Contains(word) ||
                        (company.Description != null && company.Description.Contains(word))
                    )
                );
            return filters;
        }

        private static IQueryable<Company> ApplyOrderBy(IQueryable<Company> query, GetPersonCompaniesRequest request)
        {
            switch (request.OrderBy)
            {
                case CompaniesOrderBy.Name:
                    return request.Ascending ?
                        query.OrderBy(company => company.Name) :
                        query.OrderByDescending(company => company.Name);
                default:
                    return request.Ascending ?
                        query.OrderBy(company => company.Created) :
                        query.OrderByDescending(company => company.Created);
            }
        }

        private static GetPersonCompaniesResponse InvalidResponse(HttpCode code)
        {
            return new GetPersonCompaniesResponse
            {
                Result = new GetPersonCompaniesQueryResult
                {
                    Items = [],
                    TotalCount = 0,
                },
                IsCorrect = false,
                HttpCode = code,
            };
        }

        private static GetPersonCompaniesResponse ValidResponse(
            HttpCode code,
            GetPersonCompaniesQueryResult result)
        {
            return new GetPersonCompaniesResponse
            {
                Result = result,
                IsCorrect = true,
                HttpCode = code,
            };
        }
    }
}
