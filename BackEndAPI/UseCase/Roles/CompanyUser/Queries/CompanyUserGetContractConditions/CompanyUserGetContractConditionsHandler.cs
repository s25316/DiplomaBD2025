using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions
{
    public class CompanyUserGetContractConditionsHandler : IRequestHandler<CompanyUserGetContractConditionsRequest, ItemsResponse<CompanyUserContractConditionAndCompanyDto>>
    {
        // Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserGetContractConditionsHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyUserContractConditionAndCompanyDto>> Handle(CompanyUserGetContractConditionsRequest request, CancellationToken cancellationToken)
        {

            // Prepare Data
            var personId = GetPersonId(request.Metadata.Claims);
            var personIdValue = personId.Value;
            var roleIds = _authorizedRoles.Select(r => (int)r);
            var now = CustomTimeProvider.Now;
            Expression<Func<Offer, bool>> getActiveOffers = offer =>
                offer.PublicationStart < now &&
                (
                    offer.PublicationEnd == null ||
                    offer.PublicationEnd > now
                );

            // Prepare Query
            var baseQuery = PrepareQuery(personId, request);
            var paginatedQuery = baseQuery.Paginate(
                request.Pagination.Page,
                request.Pagination.ItemsPerPage);

            // Execute Query 
            var selectResult = await paginatedQuery.Select(item => new
            {
                Item = item,
                TotalCount = baseQuery.Count(),
                ContractAttributes = _context.ContractAttributes
                    .Include(ca => ca.ContractParameter)
                    .ThenInclude(cp => cp.ContractParameterType)
                    .Where(ca =>
                        ca.Removed == null &&
                        ca.ContractConditionId == item.ContractConditionId)
                    .ToList(),
                OfferCount = _context.Offers
                    .Include(offer => offer.OfferConditions)
                    .Where(offer => offer.OfferConditions.Any(oc =>
                        oc.Removed == null &&
                        oc.ContractConditionId == item.ContractConditionId))
                    .Count(getActiveOffers),
                RolesCount = _context.CompanyPeople
                    .Count(role => roleIds.Any(roleId =>
                        role.CompanyId == item.CompanyId &&
                        role.PersonId == personIdValue &&
                        role.RoleId == roleId &&
                        role.Deny == null
                    )),
            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyUserContractConditionAndCompanyDto>();
            foreach (var item in selectResult)
            {
                if (item.Item.Company.Removed.HasValue)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }
                if (item.RolesCount == 0)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }

                item.Item.ContractAttributes = item.ContractAttributes;
                items.Add(new CompanyUserContractConditionAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    ContractCondition = _mapper.Map<CompanyUserContractConditionDto>(item.Item),
                    OfferCount = item.OfferCount,
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<CompanyUserContractConditionAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyUserContractConditionAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyUserContractConditionAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        private IQueryable<ContractCondition> PrepareBaseQuery()
        {
            return _context.ContractConditions
                .Include(ot => ot.Company)
                .AsNoTracking();
        }

        private IQueryable<ContractCondition> PrepareQuery(
            PersonId personId,
            CompanyUserGetContractConditionsRequest request)
        {
            var query = PrepareBaseQuery();

            if (request.ContractConditionId.HasValue)
            {
                return query.Where(cc =>
                    cc.ContractConditionId == request.ContractConditionId);
            }
            if (request.CompanyId.HasValue ||
                request.CompanyQueryParameters.HasValue)
            {
                query = query.Where(cc => _context.Companies
                    .WhereIdentificationData(
                        request.CompanyId,
                        request.CompanyQueryParameters)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }
            else
            {
                query = query.Where(cc => cc.Company.Removed == null)
                    .Where(cc => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == cc.CompanyId));
            }

            query = WhereText(query, request.SearchText);
            query = WhereContractParameters(query, request.ContractParameterIds);
            query = query.WhereSalary(request.SalaryParameters);
            query = query.Where(cc => request.ShowRemoved
                ? cc.Removed != null
                : cc.Removed == null);

            return OrderBy(
                query,
                request.ContractParameterIds,
                request.ShowRemoved,
                request.OrderBy,
                request.Ascending);
        }

        private IQueryable<ContractCondition> WhereText(
            IQueryable<ContractCondition> query,
            string? searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                return query.Where(cc => _context.Companies
                    .WhereText(searchText)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }
            return query;
        }

        private IQueryable<ContractCondition> WhereContractParameters(
            IQueryable<ContractCondition> query,
            IEnumerable<int> contractParameterIds)
        {
            if (contractParameterIds.Any())
            {
                return query.Where(cc =>
                    contractParameterIds.Any(contractParameterId =>
                         _context.ContractAttributes.Any(ca =>
                            ca.ContractConditionId == cc.ContractConditionId &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.Removed == null
                         )
                    ));
            }
            return query;
        }

        private IQueryable<ContractCondition> OrderBy(
            IQueryable<ContractCondition> query,
            IEnumerable<int> contractParameterIds,
            bool showRemoved,
            CompanyUserContractConditionOrderBy orderBy,
            bool ascending)
        {
            if (showRemoved && orderBy == CompanyUserContractConditionOrderBy.ContractConditionRemoved)
            {
                return ascending
                        ? query.OrderBy(cc => cc.Removed)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.Removed)
                            .ThenByDescending(cc => cc.Created);
            }

            if (contractParameterIds.Any() &&
                orderBy == CompanyUserContractConditionOrderBy.ContractParameters)
            {
                return ascending
                    ? query.OrderBy(cc => _context.ContractAttributes
                        .Count(ca => contractParameterIds.Any(contractParameterId =>
                            ca.ContractConditionId == cc.ContractConditionId &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.Removed == null
                        ))
                    ).ThenBy(ot => ot.Created)
                    : query.OrderByDescending(cc => _context.ContractAttributes
                        .Count(ca => contractParameterIds.Any(contractParameterId =>
                            ca.ContractConditionId == cc.ContractConditionId &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.Removed == null
                        ))
                    ).ThenByDescending(ot => ot.Created);
            }

            switch (orderBy)
            {
                case CompanyUserContractConditionOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Created)
                            .ThenBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Created)
                            .ThenByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionOrderBy.SalaryMin:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMin)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMin)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionOrderBy.SalaryMax:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMax)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMax)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionOrderBy.SalaryAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionOrderBy.SalaryPerHourMin:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionOrderBy.SalaryPerHourMax:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionOrderBy.SalaryPerHourAvg:
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
    }
}
