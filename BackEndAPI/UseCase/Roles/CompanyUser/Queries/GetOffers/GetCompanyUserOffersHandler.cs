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
using UseCase.Roles.CompanyUser.Queries.GetOffers.Request;
using UseCase.Roles.CompanyUser.Queries.GetOffers.Response;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers
{
    public class GetCompanyUserOffersHandler : IRequestHandler<GetCompanyUserOffersRequest, GetCompanyUserOffersResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserOffersHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyUserOffersResponse> Handle(GetCompanyUserOffersRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = BuildQuery(personId, request);

            var selector = BuildSelector(personId, query);
            var selectResults = await query
                .Paginate(request.Page, request.ItemsPerPage)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(selectResults);
        }

        // Private Static Methods 
        private sealed class SelectResult
        {
            public required Offer Offer { get; init; }
            public required int AuthorizeRolesCount { get; init; }
            public required int TotalCount { get; init; }
        }

        private static Expression<Func<Offer, SelectResult>> BuildSelector(
            PersonId personId,
            IQueryable<Offer> totalCountQuery)
        {
            return offer => new SelectResult
            {
                Offer = offer,
                AuthorizeRolesCount = offer.OfferConnections
                    .First(x => x.Removed == null)
                    .OfferTemplate.Company.CompanyPeople.Count(role =>
                    _authorizedRoles.Any(roleId =>
                        role.PersonId == personId.Value &&
                        role.RoleId == (int)roleId &&
                        role.Deny == null
                    )
                ),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static Expression<Func<Offer, bool>> BuildOfferIdFilter(Guid offerId)
        {
            return offer => offer.OfferId == offerId;
        }

        private static Expression<Func<Offer, bool>> BuildCompaniesFilter(
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs)
        {
            if (companyId.HasValue)
            {
                return offer => offer.OfferConnections
                    .Any(oc =>
                        oc.OfferTemplate.Company.CompanyId == companyId
                );
            }

            return offer => offer.OfferConnections.Any(oc =>
                (
                    (regon == null || oc.OfferTemplate.Company.Regon == regon) &&
                    (nip == null || oc.OfferTemplate.Company.Nip == nip) &&
                    (krs == null || oc.OfferTemplate.Company.Krs == krs)
                ));
        }

        private static Expression<Func<Offer, bool>> BuildBranchIdFilter(Guid branchId)
        {
            return offer => offer.BranchId.HasValue && offer.BranchId == branchId;
        }

        private static Expression<Func<Offer, bool>> BuildOffersWithBranchesFilter()
        {
            return offer => offer.Branch != null;
        }

        private static Expression<Func<Offer, bool>> BuildOfferTemplateIdFilter(Guid offerTemplateId)
        {
            return offer =>
                offer.OfferConnections.Any(oc =>
                    oc.OfferTemplateId == offerTemplateId
            );
        }

        private static Expression<Func<Offer, bool>> BuildSkillIdsFilter(IEnumerable<int> skillIds)
        {
            return offer =>
                !skillIds.Any() ||
                skillIds.Any(skillId =>
                    offer.OfferConnections.Any(oc =>
                        oc.OfferTemplate.OfferSkills.Any(skill =>
                                skill.SkillId == skillId
                            )
                        )
                    );
        }

        private static Expression<Func<Offer, bool>> BuildContractConditionIdFilter(
           Guid contractConditionId)
        {
            return offer => offer.OfferConditions.Any(oc =>
                    oc.ContractConditionId == contractConditionId
                );
        }

        private static Expression<Func<Offer, bool>> BuildContractConditionsFilter(
            GetCompanyUserOffersRequest request)
        {
            return offer =>
                !offer.OfferConditions.Any() ||
                offer.OfferConditions.Any(oc =>
                    (
                        !request.IsNegotiable.HasValue ||
                        (
                            request.IsNegotiable == true
                                ? oc.ContractCondition.IsNegotiable
                                : !oc.ContractCondition.IsNegotiable

                        )
                    ) &&
                    (
                        !request.IsPaid.HasValue ||
                        (
                            request.IsPaid == true
                                ? oc.ContractCondition.SalaryMin > 0
                                : oc.ContractCondition.SalaryMax <= 0

                        )
                    ) &&
                    (
                        !request.SalaryPerHourMin.HasValue ||
                        (
                            oc.ContractCondition.SalaryMin / oc.ContractCondition.HoursPerTerm >= request.SalaryPerHourMin
                        )
                    ) &&
                    (
                        !request.SalaryPerHourMax.HasValue ||
                        (
                            oc.ContractCondition.SalaryMax / oc.ContractCondition.HoursPerTerm <= request.SalaryPerHourMax
                        )
                    ) &&
                    (
                        !request.SalaryMin.HasValue ||
                        (
                            oc.ContractCondition.SalaryMin >= request.SalaryMin
                        )
                    ) &&
                    (
                        !request.SalaryMax.HasValue ||
                        (
                            oc.ContractCondition.SalaryMax <= request.SalaryMax
                        )
                    ) &&
                    (
                        !request.HoursMin.HasValue ||
                        (
                            oc.ContractCondition.HoursPerTerm >= request.HoursMin
                        )
                    ) &&
                    (
                        !request.HoursMax.HasValue ||
                        (
                            oc.ContractCondition.HoursPerTerm <= request.HoursMax
                        )
                    ) &&
                    (
                        !request.ParameterIds.Any() ||
                        request.ParameterIds.Any(paramId =>
                            oc.ContractCondition.ContractAttributes.Any(atribute =>
                                atribute.ContractParameterId == paramId
                        ))
                    ));
        }

        private static Expression<Func<Offer, bool>> BuildSearchTextFilter(
            IEnumerable<string> searchWords)
        {
            return offer => !searchWords.Any() || searchWords.Any(word =>
                offer.OfferConnections.Any(oc =>
                    oc.OfferTemplate.Name.Contains(word) ||
                    oc.OfferTemplate.Description.Contains(word) ||
                    (
                        oc.OfferTemplate.Company.Name != null &&
                        oc.OfferTemplate.Company.Name.Contains(word)
                    ) ||
                    (
                        oc.OfferTemplate.Company.Description != null &&
                        oc.OfferTemplate.Company.Description.Contains(word)
                    )) ||
                (
                    offer.Branch != null &&
                    (
                        (
                            offer.Branch.Name != null &&
                            offer.Branch.Name.Contains(word)
                        ) ||
                        (
                            offer.Branch.Description != null &&
                            offer.Branch.Description.Contains(word)
                        )
                    )
                )
            );
        }

        private static GetCompanyUserOffersResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserOffersResponse
            {
                Result = new ResponseQueryResultTemplate<OfferDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserOffersResponse ValidResponse(
            HttpCode code,
            IEnumerable<OfferDto> items,
            int totalCount)
        {
            return new GetCompanyUserOffersResponse
            {
                Result = new ResponseQueryResultTemplate<OfferDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(GetCompanyUserOffersRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private IQueryable<Offer> BaseQuery()
        {
            return _context.Offers
                .Include(o => o.OfferConditions)
                .ThenInclude(oc => oc.ContractCondition)
                .ThenInclude(oc => oc.ContractAttributes)
                .ThenInclude(oc => oc.ContractParameter)
                .ThenInclude(oc => oc.ContractParameterType)


                .Include(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.Street)

                .Include(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.City)
                .ThenInclude(oc => oc.State)
                .ThenInclude(oc => oc.Country)


                .Include(o => o.OfferConnections)
                .ThenInclude(oc => oc.OfferTemplate)
                .ThenInclude(oc => oc.Company)
                .ThenInclude(oc => oc.CompanyPeople)

                .Include(o => o.OfferConnections)
                .ThenInclude(oc => oc.OfferTemplate)
                .ThenInclude(oc => oc.OfferSkills)
                .ThenInclude(oc => oc.Skill)
                .ThenInclude(oc => oc.SkillType)
                .AsNoTracking();
        }

        private IQueryable<Offer> BuildQuery(
            PersonId personId,
            GetCompanyUserOffersRequest request)
        {
            var query = BaseQuery();
            if (request.OfferId.HasValue)
            {
                var offerFilter = BuildOfferIdFilter(request.OfferId.Value);
                return query.Where(offerFilter);
            }

            // Filter by Companies even we are haven`t access 
            if (request.CompanyId.HasValue ||
                    request.Regon != null ||
                    request.Nip != null ||
                    request.Krs != null)
            {
                var companiesFilter = BuildCompaniesFilter(
                    request.CompanyId,
                    request.Regon,
                    request.Nip,
                    request.Krs);
                query = query.Where(companiesFilter);
            }
            // Filter by Branches even we are haven`t access 
            else if (request.BranchId.HasValue)
            {
                var branchIdFilter = BuildBranchIdFilter(request.BranchId.Value);
                query = query.Where(branchIdFilter);
            }
            // Filter by OfferTemplate even we are haven`t access 
            else if (request.OfferTemplateId.HasValue)
            {
                var offerTemplateId = BuildOfferTemplateIdFilter(request.OfferTemplateId.Value);
                query = query.Where(offerTemplateId);
            }
            // Filter by ContractCondition even we are haven`t access 
            else if (request.ContractConditionId.HasValue)
            {
                var contractConditionIdFilter = BuildContractConditionIdFilter(request.ContractConditionId.Value);
                query = query.Where(contractConditionIdFilter);
            }
            else
            {
                query = query.Where(offer => offer.OfferConnections.Any(oc =>
                    oc.Removed == null &&
                    oc.OfferTemplate.Company.Removed == null &&
                    oc.OfferTemplate.Company.CompanyPeople.Any(role =>
                        _authorizedRoles.Any(roleId =>
                            role.PersonId == personId.Value &&
                            role.RoleId == (int)roleId &&
                            role.Deny == null
                        )
                )));
            }


            // If Lon && Lat != null
            if (request.Lon.HasValue && request.Lat.HasValue)
            {
                var offersWithBranchesFilter = BuildOffersWithBranchesFilter();
                query = query.Where(offersWithBranchesFilter);
            }
            // SkillIds
            var skillIdsFilter = BuildSkillIdsFilter(request.SkillIds);
            // Contract Conditions parameters (sal long ids)
            var contractConditionsFilter = BuildContractConditionsFilter(request);
            // Search text Part
            var searchWords = CustomStringProvider.Split(request.SearchText);
            var searchTextFilter = BuildSearchTextFilter(searchWords);

            query = query.Where(skillIdsFilter)
                .Where(contractConditionsFilter)
                .Where(searchTextFilter);

            return query;
        }

        private GetCompanyUserOffersResponse PrepareResponse(List<SelectResult> items)
        {
            if (!items.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<OfferDto>();
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
                if (item.Offer.OfferConnections.First().OfferTemplate.Company.Removed != null)
                {
                    return InvalidResponse(HttpCode.Gone);
                }

                dtos.Add(_mapper.Map<OfferDto>(item.Offer));
            }

            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }
    }
}
