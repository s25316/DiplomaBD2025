using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Guests.Queries.GuestGetContractConditions.Request;
using UseCase.Roles.Guests.Queries.GuestGetContractConditions.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.ExtensionMethods.EF.Offers;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.Guest;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetContractConditions
{
    public class GuestGetContractConditionsHandler : IRequestHandler<GuestGetContractConditionsRequest, ItemsResponse<GuestContractConditionAndCompanyDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        //private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GuestGetContractConditionsHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<GuestContractConditionAndCompanyDto>> Handle(GuestGetContractConditionsRequest request, CancellationToken cancellationToken)
        {
            // Prepare Data
            var getActiveOffersExpression = OfferEFExpressions.ActiveOffersExpression();

            // Prepare Query
            var baseQuery = PrepareQuery(request, getActiveOffersExpression);
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
                    .Count(getActiveOffersExpression),

            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<GuestContractConditionAndCompanyDto>();
            foreach (var item in selectResult)
            {
                if (item.Item.Company.Removed.HasValue ||
                    (item.Item.Removed.HasValue && item.OfferCount == 0))
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }

                if (item.Item.Company.Blocked.HasValue)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }

                item.Item.ContractAttributes = item.ContractAttributes;
                items.Add(new GuestContractConditionAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    ContractCondition = _mapper.Map<GuestContractConditionDto>(item.Item),
                    OfferCount = item.OfferCount,
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<GuestContractConditionAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<GuestContractConditionAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<GuestContractConditionAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private IQueryable<ContractCondition> PrepareBaseQuery()
        {
            return _context.ContractConditions
                .Include(ot => ot.Company)
                .AsNoTracking();
        }


        private IQueryable<ContractCondition> PrepareQuery(
            GuestGetContractConditionsRequest request,
            Expression<Func<Offer, bool>> getActiveOffers)
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
                query = query.Where(cc =>
                    cc.Company.Removed == null &&
                    cc.Company.Blocked == null)
                    .Where(cc =>
                    cc.Removed == null ||
                    (
                        cc.Removed != null &&
                        _context.Offers
                        .Where(offer => offer.OfferConditions.Any(oc =>
                            oc.Removed == null &&
                            oc.ContractConditionId == cc.ContractConditionId
                        ))
                        .Any(getActiveOffers)
                    ));
            }

            query = WhereText(query, request.SearchText);
            query = WhereContractParameters(query, request.ContractParameterIds);
            query = query.WhereSalary(request.SalaryParameters);

            return OrderBy(
                query,
                request.ContractParameterIds,
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
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractConditionId == cc.ContractConditionId
                         )
                    ));
            }
            return query;
        }

        private IQueryable<ContractCondition> OrderBy(
            IQueryable<ContractCondition> query,
            IEnumerable<int> contractParameterIds,
            GuestContractConditionOrderBy orderBy,
            bool ascending)
        {
            if (contractParameterIds.Any() &&
                orderBy == GuestContractConditionOrderBy.ContractParameters)
            {
                return ascending
                    ? query.OrderBy(cc => _context.ContractAttributes
                        .Count(ca => contractParameterIds.Any(contractParameterId =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractConditionId == cc.ContractConditionId
                        ))
                    ).ThenBy(ot => ot.Created)
                    : query.OrderByDescending(cc => _context.ContractAttributes
                        .Count(ca => contractParameterIds.Any(contractParameterId =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractConditionId == cc.ContractConditionId
                        ))
                    ).ThenByDescending(ot => ot.Created);
            }

            switch (orderBy)
            {
                case GuestContractConditionOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Name);
                case GuestContractConditionOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Created)
                            .ThenBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Created)
                            .ThenByDescending(cc => cc.Company.Name);
                case GuestContractConditionOrderBy.SalaryMin:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMin)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMin)
                            .ThenByDescending(cc => cc.Created);
                case GuestContractConditionOrderBy.SalaryMax:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMax)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMax)
                            .ThenByDescending(cc => cc.Created);
                case GuestContractConditionOrderBy.SalaryAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenByDescending(cc => cc.Created);
                case GuestContractConditionOrderBy.SalaryPerHourMin:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case GuestContractConditionOrderBy.SalaryPerHourMax:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case GuestContractConditionOrderBy.SalaryPerHourAvg:
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
