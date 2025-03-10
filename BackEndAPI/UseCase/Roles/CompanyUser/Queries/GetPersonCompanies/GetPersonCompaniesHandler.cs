using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies
{
    public class GetPersonCompaniesHandler : IRequestHandler<GetPersonCompaniesRequest, GetPersonCompaniesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


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
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(personId, query);
            var selectedValues = await query
                .Paginate(request.Page, request.ItemsPerPage)
                .Select(selector)
                .ToListAsync(cancellationToken);

            var totalCount = -1;
            var isForbidden = false;
            var isRemoved = false;
            var dtos = new List<CompanyDto>();
            for (int i = 0; i < selectedValues.Count; i++)
            {
                var selectedValue = selectedValues[i];
                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                if (selectedValue.AuthorizeRolesCount == 0)
                {
                    isForbidden = true;
                    break;
                }
                if (selectedValue.Company.Removed != null)
                {
                    isRemoved = true;
                    break;
                }
                dtos.Add(_mapper.Map<CompanyDto>(selectedValue.Company));
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

        // Private Methods
        private PersonId GetPersonId(GetPersonCompaniesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private IQueryable<Company> BuildBaseQuery()
        {
            return _context.Companies
                .Include(c => c.CompanyPeople)
                .AsNoTracking();
        }

        private IQueryable<Company> BuildQuery(
            GetPersonCompaniesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();

            // Parameters Defines The Company
            if (request.CompanyId != null ||
                request.Regon != null ||
                request.Nip != null ||
                request.Krs != null)
            {
                var filters = BuildCompanyFilter(request);
                query = query.Where(filters);
            }
            else
            {
                var filters = BuildSearchTextFilter(request.SearchText);
                query = query
                    .Where(filters)
                    .Where(company => company.CompanyPeople.Any(role =>
                        _authorizedRoles.Any(roleId =>
                            role.RoleId == (int)roleId &&
                            role.PersonId == personId.Value &&
                            role.Deny == null
                        )));
                query = ApplyOrderBy(query, request.OrderBy, request.Ascending);
            }
            return query;
        }

        // Private Static Methods and private Class for EF selection
        private sealed class CompanySelectionResult
        {
            public required Company Company { get; set; }
            public required int AuthorizeRolesCount { get; set; }
            public required int TotalCount { get; set; }
        }

        private static Expression<Func<Company, CompanySelectionResult>> BuildSelector(
            PersonId personId,
            IQueryable<Company> totalCountQuery)
        {
            return company => new CompanySelectionResult
            {
                Company = company,
                AuthorizeRolesCount = company.CompanyPeople.Count(role =>
                    _authorizedRoles.Any(roleId =>
                        role.RoleId == (int)roleId &&
                        role.PersonId == personId.Value &&
                        role.Deny == null
                    )),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static Expression<Func<Company, bool>> BuildCompanyFilter(
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

        private static Expression<Func<Company, bool>> BuildSearchTextFilter(string? searchText)
        {
            var searchWords = CustomStringProvider.Split(searchText);

            return company =>
                company.Name != null &&
                company.Nip != null &&
                company.Regon != null &&
                company.Removed == null &&
                (
                    !searchWords.Any() ||
                    searchWords.Any(word =>
                        company.Name.Contains(word) ||
                        (company.Description != null && company.Description.Contains(word))
                    )
                );
        }

        private static IQueryable<Company> ApplyOrderBy(
            IQueryable<Company> query,
            CompaniesOrderBy orderBy,
            bool ascending)
        {
            switch (orderBy)
            {
                case CompaniesOrderBy.Name:
                    return ascending ?
                        query.OrderBy(company => company.Name) :
                        query.OrderByDescending(company => company.Name);
                default:
                    return ascending ?
                        query.OrderBy(company => company.Created) :
                        query.OrderByDescending(company => company.Created);
            }
        }

        private static GetPersonCompaniesResponse InvalidResponse(HttpCode code)
        {
            return new GetPersonCompaniesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyDto>
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
            IEnumerable<CompanyDto> items,
            int totalCount)
        {
            return new GetPersonCompaniesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                IsCorrect = true,
                HttpCode = code,
            };
        }
    }
}
