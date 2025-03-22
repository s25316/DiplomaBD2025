using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.CustomProviders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOffers.Request;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.ExtensionMethods.EF.Offers;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers
{
    public class GetCompanyUserOffersHandler :
        GetCompanyUserGenericsBase<Offer, OfferDto>,
        IRequestHandler<GetCompanyUserOffersRequest, GetCompanyUserGenericItemsResponse<OfferDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserOffersHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector
            ) : base(context, mapper, authenticationInspector)
        { }


        // Methods
        public async Task<GetCompanyUserGenericItemsResponse<OfferDto>> Handle(GetCompanyUserOffersRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = BuildQuery(personId, request);

            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                offer => offer.OfferConnections
                    .OrderByDescending(oc => oc.Created)
                    .First(oc => oc.Removed == null)
                    .OfferTemplate.Company.CompanyPeople);

            var selectResults = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectResults,
                offer => offer.OfferConnections
                        .OrderByDescending(oc => oc.Created)
                        .First(oc => oc.Removed == null)
                        .OfferTemplate.Company.Removed != null,
                offer => _mapper.Map<OfferDto>(offer)
                );
        }

        // Private Non  Static Methods 
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
                return query.Where(offer => offer.OfferId == request.OfferId.Value);
            }

            // Filter by Companies even we are haven`t access 
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                query = query.Where(offer => _context.Companies
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters)
                    .Any(company => offer.OfferConnections
                        .Any(oc => oc.OfferTemplate.CompanyId == company.CompanyId)));
            }
            // Filter by Branches even we are haven`t access 
            else if (request.BranchId.HasValue)
            {
                query = query.Where(offer => offer.BranchId == request.BranchId);
            }
            // Filter by OfferTemplate even we are haven`t access 
            else if (request.OfferTemplateId.HasValue)
            {
                query = query.Where(offer => offer.OfferConnections
                    .Any(oc => oc.OfferTemplateId == request.OfferTemplateId));
            }
            // Filter by ContractCondition even we are haven`t access 
            else if (request.ContractConditionId.HasValue)
            {
                query = query.Where(offer => offer.OfferConditions
                    .Any(oc => oc.ContractConditionId == request.ContractConditionId));
            }
            else
            {
                query = query.Where(offer => offer.OfferConnections.Any(oc =>
                    _context.CompanyPeople.WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == oc.OfferTemplate.CompanyId)
                ));
            }


            // If Lon && Lat != null
            if (request.GeographyPoint.HasValue())
            {
                query = query.Where(offer => offer.BranchId != null);
            }
            // SkillIds
            if (request.SkillIds.Any())
            {
                query = query.Where(offer => offer.OfferConnections.Any(oc =>
                    _context.OfferTemplates
                        .SkillsFilter(request.SkillIds)
                        .Any(offerTemplate => oc.OfferTemplateId == offerTemplate.OfferTemplateId)
                    ));
            }
            // Contract Conditions
            if (request.ContractParameterIds.Any() ||
                request.SalaryParameters.ContainsAny())
            {
                query = query.Where(offer => offer.OfferConditions.Any(oc =>
                    _context.ContractConditions
                        .ContractParametersAndSalaryFilter(
                            request.ContractParameterIds,
                            request.SalaryParameters)
                        .Any(cc => cc.ContractConditionId == oc.ContractConditionId)
                ));
            }

            // Search text Part
            var searchWords = CustomStringProvider.Split(request.SearchText);
            if (searchWords.Any())
            {
                query = query.SearchTextFilter(searchWords);
            }

            query = query.OfferParametersFilter(request.OfferParameters);

            return query;
        }
    }
}
