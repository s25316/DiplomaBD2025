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
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions
{
    public class GetCompanyUserContractConditionsHandler : IRequestHandler<GetCompanyUserContractConditionsRequest, GetCompanyUserContractConditionsResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserContractConditionsHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyUserContractConditionsResponse> Handle(GetCompanyUserContractConditionsRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = PrepareQuery(personId, request);
            var selector = BuildSelector(personId, query);
            var selectResult = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(selectResult);
        }

        // Private Static Methods 
        private sealed class SelectResult
        {
            public required ContractCondition ContractCondition { get; init; }
            public required int AuthorizeRolesCount { get; init; }
            public required int TotalCount { get; init; }
        }

        private static Expression<Func<ContractCondition, SelectResult>> BuildSelector(
            PersonId personId,
            IQueryable<ContractCondition> totalCountQuery)
        {
            var personIdValue = personId.Value;
            var roleIds = _authorizedRoles.Select(r => (int)r);
            return cc => new SelectResult
            {
                ContractCondition = cc,
                AuthorizeRolesCount = cc.Company.CompanyPeople.Count(role =>
                    roleIds.Any(roleId =>
                        role.PersonId == personIdValue &&
                        role.RoleId == roleId &&
                        role.Deny == null
                    )
                ),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static GetCompanyUserContractConditionsResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserContractConditionsResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndContractConditionDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserContractConditionsResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyAndContractConditionDto> items,
            int totalCount)
        {
            return new GetCompanyUserContractConditionsResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndContractConditionDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }

        private static IQueryable<ContractCondition> ApplyOrderBy(
            IQueryable<ContractCondition> query,
            CompanyUserContractConditionsOrderBy orderBy,
            bool ascending,
            bool showRemoved)
        {
            if (showRemoved && orderBy == CompanyUserContractConditionsOrderBy.ContractConditionRemoved)
            {
                return ascending
                        ? query.OrderBy(cc => cc.Removed)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.Removed)
                            .ThenByDescending(cc => cc.Created);
            }

            switch (orderBy)
            {
                case CompanyUserContractConditionsOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionsOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Created)
                            .ThenBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Created)
                            .ThenByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionsOrderBy.SalaryMin:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMin)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMin)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryMax:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMax)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMax)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourMin:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourMax:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => ((cc.SalaryMax + cc.SalaryMin) / 2) / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => ((cc.SalaryMax + cc.SalaryMin) / 2) / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                default://case CompanyUserContractConditionsOrderBy.ContractConditionCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Created)
                            .ThenBy(cc => cc.SalaryMin)
                        : query.OrderByDescending(cc => cc.Created)
                            .ThenByDescending(cc => cc.SalaryMin);
            }
        }

        // Private Non Static Methods 
        private PersonId GetPersonId(GetCompanyUserContractConditionsRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private IQueryable<ContractCondition> BaseQuery()
        {
            return _context.ContractConditions
                .Include(cc => cc.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(cc => cc.ContractAttributes)
                .ThenInclude(c => c.ContractParameter)
                .ThenInclude(c => c.ContractParameterType)
                .AsNoTracking();
        }

        private IQueryable<ContractCondition> PrepareQuery(
            PersonId personId,
            GetCompanyUserContractConditionsRequest request)
        {
            // Base Configuration
            var query = BaseQuery();

            // Filter ContractCondition if have no access to it
            if (request.ContractConditionId.HasValue)
            {
                return query.Where(cc => cc.ContractConditionId == request.ContractConditionId);
            }
            // Filter Companies if have no access to it
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                query = query.Where(cc => _context.Companies
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }
            // Filter only to which have access
            else
            {
                query = query.Where(cc =>
                    cc.Company.Removed == null)
                    .Where(cc => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == cc.CompanyId));
            }

            // Show only removed, or only not removed
            query = query.Where(cc => request.ShowRemoved
                ? cc.Removed != null
                : cc.Removed == null);

            // Search Text Filter 
            var searchWords = CustomStringProvider.Split(request.SearchText);
            if (searchWords.Any())
            {
                query = query.Where(cc => _context.Companies
                    .SearchTextFilter(searchWords)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }

            // Contract Parameters And Salary
            query = query.ContractParametersAndSalaryFilter(
                request.ContractParameterIds,
                request.SalaryParameters);

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending,
                request.ShowRemoved);

            return query;
        }

        private GetCompanyUserContractConditionsResponse PrepareResponse(List<SelectResult> items)
        {
            if (!items.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<CompanyAndContractConditionDto>();
            foreach (var item in items)
            {
                if (totalCount < 0)
                {
                    totalCount = item.TotalCount;
                }
                if (item.AuthorizeRolesCount == 0)
                {
                    return InvalidResponse(HttpCode.Forbidden);
                }
                if (item.ContractCondition.Company.Removed != null)
                {
                    return InvalidResponse(HttpCode.Gone);
                }
                dtos.Add(new CompanyAndContractConditionDto
                {
                    Company = _mapper.Map<CompanyDto>(item.ContractCondition.Company),
                    ContractCondition = _mapper.Map<ContractConditionDto>(item.ContractCondition),
                });
            }
            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }
    }
}
