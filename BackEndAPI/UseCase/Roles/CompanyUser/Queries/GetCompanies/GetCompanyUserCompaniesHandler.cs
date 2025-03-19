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
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies
{
    public class GetCompanyUserCompaniesHandler : IRequestHandler<GetCompanyUserCompaniesRequest, GetCompanyUserCompaniesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserCompaniesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyUserCompaniesResponse> Handle(GetCompanyUserCompaniesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(personId, query);
            var selectedValues = await query
                .Paginate(request.Page, request.ItemsPerPage)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(selectedValues);
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
            GetCompanyUserCompaniesRequest request)
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

        private static Expression<Func<Company, bool>> BuildSearchTextFilter(
            IEnumerable<string> searchWords)
        {
            return company =>
                company.Name != null &&
                (
                    !searchWords.Any() ||
                    searchWords.Any(word =>
                        (company.Name != null && company.Name.Contains(word)) ||
                        (company.Description != null && company.Description.Contains(word))
                ));
        }

        private static IQueryable<Company> ApplyOrderBy(
            IQueryable<Company> query,
            CompanyUserCompaniesOrderBy orderBy,
            bool ascending)
        {
            switch (orderBy)
            {
                case CompanyUserCompaniesOrderBy.Name:
                    return ascending ?
                        query.OrderBy(company => company.Name) :
                        query.OrderByDescending(company => company.Name);
                default:
                    return ascending ?
                        query.OrderBy(company => company.Created) :
                        query.OrderByDescending(company => company.Created);
            }
        }

        private static GetCompanyUserCompaniesResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserCompaniesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserCompaniesResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyDto> items,
            int totalCount)
        {
            return new GetCompanyUserCompaniesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }


        // Private Non Static Methods
        private PersonId GetPersonId(GetCompanyUserCompaniesRequest request)
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
            GetCompanyUserCompaniesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();

            // Parameters Defines The Company
            if (request.CompanyId != null ||
                request.Regon != null ||
                request.Nip != null ||
                request.Krs != null)
            {
                var companyFilters = BuildCompanyFilter(request);
                return query.Where(companyFilters);
            }

            var searchWords = CustomStringProvider.Split(request.SearchText);
            var filters = BuildSearchTextFilter(searchWords);
            query = query
                .Where(filters)
                .Where(company =>
                    company.Removed == null &&
                    company.CompanyPeople.Any(role =>
                        _authorizedRoles.Any(roleId =>
                            role.RoleId == (int)roleId &&
                            role.PersonId == personId.Value &&
                            role.Deny == null
                        )));

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending);

            return query;
        }

        private GetCompanyUserCompaniesResponse PrepareResponse(List<CompanySelectionResult> items)
        {
            if (items.Count == 0)
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<CompanyDto>();

            for (int i = 0; i < items.Count; i++)
            {
                var selectedValue = items[i];
                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                if (selectedValue.AuthorizeRolesCount == 0)
                {
                    return InvalidResponse(HttpCode.Forbidden);
                }
                if (selectedValue.Company.Removed != null)
                {
                    return InvalidResponse(HttpCode.Gone);
                }
                dtos.Add(_mapper.Map<CompanyDto>(selectedValue.Company));
            }

            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }
    }
}
