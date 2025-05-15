using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Security.Claims;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.ExtensionMethods.EF.Offers;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers
{
    public class CompanyUserGetOffersHandler : IRequestHandler<CompanyUserGetOffersRequest, ItemsResponse<CompanyUserOfferFullDto>>
    {
        // Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserGetOffersHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyUserOfferFullDto>> Handle(CompanyUserGetOffersRequest request, CancellationToken cancellationToken)
        {
            // Prepare Data
            var personId = GetPersonId(request.Metadata.Claims);
            var personIdValue = personId.Value;
            var roleIds = _authorizedRoles.Select(r => (int)r);

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
                OfferTemplate = _context.OfferConnections
                    .Include(oc => oc.OfferTemplate)
                    .ThenInclude(ot => ot.Company)
                    .Where(oc =>
                        oc.Removed == null &&
                        oc.OfferId == item.OfferId)
                    .OrderByDescending(oc => oc.Created)
                    .Select(oc => new
                    {
                        Item = oc.OfferTemplate,
                        Skills = _context.OfferSkills
                            .Include(os => os.Skill)
                            .ThenInclude(s => s.SkillType)
                            .Where(os =>
                                os.Removed == null &&
                                os.OfferTemplateId == oc.OfferTemplateId)
                            .ToList(),
                        RolesCount = _context.CompanyPeople
                            .Count(role => roleIds.Any(roleId =>
                                role.Deny == null &&
                                role.RoleId == roleId &&
                                role.PersonId == personIdValue &&
                                role.CompanyId == oc.OfferTemplate.CompanyId)),
                    }).First(),
                ContractConditions = _context.OfferConditions
                    .Include(oc => oc.ContractCondition)
                    .Where(oc =>
                        oc.Removed == null &&
                        oc.OfferId == item.OfferId)
                    .Select(oc => new
                    {
                        Item = oc.ContractCondition,
                        ContractAttributes = _context.ContractAttributes
                            .Include(ca => ca.ContractParameter)
                            .ThenInclude(ca => ca.ContractParameterType)
                            .AsQueryable()
                            .Where(ca =>
                                ca.Removed == null &&
                                ca.ContractConditionId == oc.ContractConditionId)
                            .ToList(),
                    }).ToList(),

            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.NotFound, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyUserOfferFullDto>();
            foreach (var item in selectResult)
            {
                var company = item.OfferTemplate.Item.Company;
                if (company.Removed.HasValue)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }

                if (item.OfferTemplate.RolesCount == 0)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }

                // Prepare Data for Mapping
                item.OfferTemplate.Item.OfferSkills = item.OfferTemplate.Skills;
                var contractConditions = new List<ContractCondition>();
                foreach (var contractCondition in item.ContractConditions)
                {
                    contractCondition.Item.ContractAttributes = contractCondition.ContractAttributes;
                    contractConditions.Add(contractCondition.Item);
                }

                items.Add(new CompanyUserOfferFullDto
                {
                    Offer = _mapper.Map<OfferDto>(item.Item),
                    Branch = _mapper.Map<CompanyUserBranchDto>(item.Item.Branch),
                    Company = _mapper.Map<CompanyDto>(company),
                    OfferTemplate = _mapper.Map<CompanyUserOfferTemplateDto>(item.OfferTemplate.Item),
                    ContractConditions = _mapper.Map<IEnumerable<CompanyUserContractConditionDto>>(contractConditions),
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<CompanyUserOfferFullDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyUserOfferFullDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyUserOfferFullDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        private IQueryable<Offer> PrepareBaseQuery()
        {
            return _context.Offers
                .Include(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.Street)

                .Include(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.City)
                .ThenInclude(oc => oc.State)
                .ThenInclude(oc => oc.Country)
                .AsNoTracking();
        }

        private IQueryable<Offer> PrepareQuery(
            PersonId personId,
            CompanyUserGetOffersRequest request)
        {
            var query = PrepareBaseQuery();

            if (request.OfferId.HasValue)
            {
                return query.Where(offer =>
                offer.OfferId == request.OfferId.Value);
            }

            if (request.CompanyId.HasValue ||
               request.CompanyQueryParameters.HasValue)
            {
                query = query.Where(offer => _context.Companies
                    .WhereIdentificationData(
                        request.CompanyId,
                        request.CompanyQueryParameters)
                    .Any(company => offer.OfferConnections
                        .Any(oc => oc.OfferTemplate.CompanyId == company.CompanyId)));
            }
            else if (request.BranchId.HasValue)
            {
                query = query
                    .Where(offer => offer.BranchId == request.BranchId);
            }
            else if (request.OfferTemplateId.HasValue)
            {
                query = query.Where(offer =>
                    _context.OfferConnections.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferId == offer.OfferId &&
                        oc.OfferTemplateId == request.OfferTemplateId));
            }
            else if (request.ContractConditionId.HasValue)
            {
                query = query.Where(offer =>
                    _context.OfferConditions.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferId == offer.OfferId &&
                        oc.ContractConditionId == request.ContractConditionId));
            }
            else
            {
                query = query.Where(offer =>
                    _context.OfferConnections
                    .Include(oc => oc.OfferTemplate)
                    .Any(oc =>
                        oc.OfferId == offer.OfferId &&
                        _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == oc.OfferTemplate.CompanyId)
                    ));
            }

            query = query
                .WhereOfferParameters(request.OfferQueryParameters)
                .WhereStatus(request.Status);
            query = WhereSalary(query, request.SalaryParameters);
            query = WhereContractParameters(query, request.ContractParameterIds);
            query = WhereSkills(query, request.SkillIds);
            query = WhereText(query, request.SearchText);

            return OrderBy(
                query,
                request.GeographyPoint,
                request.SkillIds,
                request.ContractParameterIds,
                request.OrderBy,
                request.Ascending);
        }

        private IQueryable<Offer> WhereSalary(
            IQueryable<Offer> query,
            SalaryQueryParametersDto parameters)
        {
            if (parameters.HasValue)
            {
                return query.Where(offer =>
                    _context.ContractConditions
                        .Include(cc => cc.OfferConditions)
                        .WhereSalary(parameters)
                        .Any(cc =>
                            cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                        )));
            }
            return query;
        }

        private IQueryable<Offer> WhereContractParameters(
            IQueryable<Offer> query,
            IEnumerable<int> contractParameterIds)
        {
            if (contractParameterIds.Any())
            {
                return query.Where(offer =>
                    contractParameterIds.Any(contractParameterId =>
                         _context.ContractAttributes
                         .Include(ca => ca.ContractCondition)
                         .ThenInclude(cc => cc.OfferConditions)
                         .Any(ca =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractCondition.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                         ))
                    ));
            }
            return query;
        }

        private IQueryable<Offer> WhereSkills(
           IQueryable<Offer> query,
           IEnumerable<int> skillIds)
        {
            if (skillIds.Any())
            {
                return query.Where(offer =>
                    skillIds.Any(skillId =>
                        _context.OfferSkills
                        .Include(os => os.OfferTemplate)
                        .ThenInclude(ot => ot.OfferConnections)
                        .Any(os =>
                            os.Removed == null &&
                            os.SkillId == skillId &&
                            os.OfferTemplate.OfferConnections.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )
                        )
                    )
                );
            }
            return query;
        }

        private IQueryable<Offer> WhereText(
            IQueryable<Offer> query,
            string? searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                return query.Where(offer =>
                    _context.OfferTemplates
                    .Include(ot => ot.Company)
                    .Include(ot => ot.OfferConnections)
                    .WhereText(searchText)
                    .Any(ot => ot.OfferConnections.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferId == offer.OfferId
                    )) ||
                    _context.Branches
                    .Include(branch => branch.Company)
                    .WhereText(searchText)
                    .Any(branch =>
                        offer.BranchId == branch.BranchId)
                );
            }
            return query;
        }

        private IQueryable<Offer> OrderBy(
            IQueryable<Offer> query,
            GeographyPointQueryParametersDto geographyPoint,
            IEnumerable<int> skillIds,
            IEnumerable<int> contractParameterIds,
            OfferOrderBy orderBy,
            bool ascending)
        {
            if (orderBy == OfferOrderBy.Point &&
                geographyPoint.Lon.HasValue &&
                geographyPoint.Lat.HasValue)
            {
                var point = new Point(
                    geographyPoint.Lon.Value,
                    geographyPoint.Lat.Value)
                { SRID = 4326 };

                return ascending
                    ? query.OrderBy(offer => offer.Branch.Address.Point.Distance(point))
                    : query.OrderByDescending(offer => offer.Branch.Address.Point.Distance(point));
            }

            if (skillIds.Any() &&
                orderBy == OfferOrderBy.Skills)
            {
                return ascending
                    ? query.OrderBy(offer => _context.OfferSkills
                        .Include(os => os.OfferTemplate)
                        .ThenInclude(ot => ot.OfferConnections)
                        .Count(skill => skillIds.Any(skillId =>
                            skill.Removed == null &&
                            skill.SkillId == skillId &&
                            skill.OfferTemplate.OfferConnections.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )
                        )))
                    : query.OrderByDescending(offer => _context.OfferSkills
                        .Include(os => os.OfferTemplate)
                        .ThenInclude(ot => ot.OfferConnections)
                        .Count(skill => skillIds.Any(skillId =>
                            skill.Removed == null &&
                            skill.SkillId == skillId &&
                            skill.OfferTemplate.OfferConnections.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )
                        )));
            }

            if (contractParameterIds.Any() &&
                orderBy == OfferOrderBy.ContractParameters)
            {
                return ascending
                    ? query.OrderBy(offer => _context.ContractAttributes
                        .Include(ca => ca.ContractCondition)
                        .ThenInclude(ca => ca.OfferConditions)
                        .Count(ca =>
                            contractParameterIds.Any(contractParameterId =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractCondition.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                        )))
                    )
                    : query.OrderByDescending(offer => _context.ContractAttributes
                        .Include(ca => ca.ContractCondition)
                        .ThenInclude(ca => ca.OfferConditions)
                        .Count(ca =>
                            contractParameterIds.Any(contractParameterId =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractCondition.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                        )))
                    );
            }

            switch (orderBy)
            {
                case OfferOrderBy.PublicationStart:
                    return ascending
                        ? query.OrderBy(offer => offer.PublicationStart)
                        : query.OrderByDescending(offer => offer.PublicationStart);

                case OfferOrderBy.PublicationEnd:
                    return ascending
                        ? query.OrderBy(offer => offer.PublicationEnd)
                        : query.OrderByDescending(offer => offer.PublicationEnd);

                case OfferOrderBy.SalaryMin:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Min(cc => cc.SalaryMin)
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Min(cc => cc.SalaryMin)
                            .Value
                        );

                case OfferOrderBy.SalaryMax:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Max(cc => cc.SalaryMax)
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Max(cc => cc.SalaryMax)
                            .Value
                        );

                case OfferOrderBy.SalaryAvg:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Average(cc => ((cc.SalaryMax + cc.SalaryMin) / 2))
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Average(cc => ((cc.SalaryMax + cc.SalaryMin) / 2))
                            .Value
                        );

                case OfferOrderBy.SalaryPerHourMin:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Min(cc => (cc.SalaryMin / cc.HoursPerTerm))
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Min(cc => (cc.SalaryMin / cc.HoursPerTerm))
                            .Value
                        );

                case OfferOrderBy.SalaryPerHourMax:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Max(cc => (cc.SalaryMax / cc.HoursPerTerm))
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Max(cc => (cc.SalaryMax / cc.HoursPerTerm))
                            .Value
                        );

                case OfferOrderBy.SalaryPerHourAvg:
                    return ascending
                        ? query.OrderBy(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Average(cc => ((cc.SalaryMax + cc.SalaryMin) / (2 * cc.HoursPerTerm)))
                            .Value
                        )
                        : query.OrderByDescending(offer =>
                            _context.ContractConditions
                            .Include(cc => cc.OfferConditions)
                            .Where(cc => cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == offer.OfferId
                            )).Average(cc => ((cc.SalaryMax + cc.SalaryMin) / (2 * cc.HoursPerTerm)))
                            .Value
                        );

                default:
                    return query;
            }
        }
    }
}
