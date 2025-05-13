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
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates
{
    public class CompanyUserGetOfferTemplatesHandler : IRequestHandler<CompanyUserGetOfferTemplatesRequest, ItemsResponse<CompanyUserOfferTemplateAndCompanyDto>>
    {
        // Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserGetOfferTemplatesHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyUserOfferTemplateAndCompanyDto>> Handle(CompanyUserGetOfferTemplatesRequest request, CancellationToken cancellationToken)
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
                Skills = _context.OfferSkills
                    .Include(os => os.Skill)
                    .ThenInclude(os => os.SkillType)
                    .Where(os =>
                        os.Removed == null &&
                        os.OfferTemplateId == item.OfferTemplateId)
                    .ToList(),
                RolesCount = _context.CompanyPeople
                    .Count(role => roleIds.Any(roleId =>
                        role.CompanyId == item.CompanyId &&
                        role.PersonId == personIdValue &&
                        role.RoleId == roleId &&
                        role.Deny == null
                    )),
                OfferCount = _context.Offers
                    .Include(offer => offer.OfferConnections)
                    .Where(offer => offer.OfferConnections.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferConnectionId == item.OfferTemplateId))
                    .Count(getActiveOffers),
            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.NotFound, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyUserOfferTemplateAndCompanyDto>();
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

                item.Item.OfferSkills = item.Skills;
                items.Add(new CompanyUserOfferTemplateAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    OfferTemplate = _mapper.Map<CompanyUserOfferTemplateDto>(item.Item),
                    OfferCount = item.OfferCount,
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<CompanyUserOfferTemplateAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyUserOfferTemplateAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyUserOfferTemplateAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        private IQueryable<OfferTemplate> PrepareBaseQuery()
        {
            return _context.OfferTemplates
                .Include(ot => ot.Company)
                .AsNoTracking();
        }

        private IQueryable<OfferTemplate> PrepareQuery(
            PersonId personId,
            CompanyUserGetOfferTemplatesRequest request)
        {
            var query = PrepareBaseQuery();

            if (request.OfferTemplateId.HasValue)
            {
                return query.Where(ot =>
                    ot.OfferTemplateId == request.OfferTemplateId.Value);
            }
            if (request.CompanyId.HasValue ||
                request.CompanyQueryParameters.HasValue)
            {
                query = query.Where(ot => _context.Companies
                    .WhereIdentificationData(
                        request.CompanyId,
                        request.CompanyQueryParameters)
                    .Any(company => company.CompanyId == ot.CompanyId));
            }
            else
            {
                query = query
                    .Where(ot => ot.Company.Removed == null)
                    .Where(ot => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == ot.CompanyId));
            }

            query = query.Where(ot => request.ShowRemoved
                ? ot.Removed != null
                : ot.Removed == null);
            query = query.WhereText(request.SearchText);
            query = WhereSkills(query, request.SkillIds);

            return OrderBy(
                query,
                request.SkillIds,
                request.ShowRemoved,
                request.OrderBy,
                request.Ascending);
        }

        private IQueryable<OfferTemplate> WhereSkills(
            IQueryable<OfferTemplate> query,
            IEnumerable<int> skillIds)
        {
            if (skillIds.Any())
            {
                return query.Where(ot =>
                    skillIds.Any(skillId =>
                        _context.OfferSkills.Any(os =>
                            os.Removed == null &&
                            os.SkillId == skillId &&
                            os.OfferTemplateId == ot.OfferTemplateId
                        )
                    )
                );
            }
            return query;
        }

        private IQueryable<OfferTemplate> OrderBy(
            IQueryable<OfferTemplate> query,
            IEnumerable<int> skillIds,
            bool showRemoved,
            CompanyUserOfferTemplateOrderBy orderBy,
            bool ascending)
        {
            if (showRemoved &&
                orderBy == CompanyUserOfferTemplateOrderBy.OfferTemplateRemoved)
            {
                return ascending
                        ? query.OrderBy(ot => ot.Removed)
                        : query.OrderByDescending(ot => ot.Removed);
            }

            if (skillIds.Any() &&
                orderBy == CompanyUserOfferTemplateOrderBy.Skills)
            {
                return ascending
                    ? query.OrderBy(ot => _context.OfferSkills
                        .Count(skill => skillIds.Any(skillId =>
                            skill.Removed == null &&
                            skill.SkillId == skillId &&
                            skill.OfferTemplateId == ot.OfferTemplateId
                        ))
                    ).ThenBy(ot => ot.Created)
                    : query.OrderByDescending(ot => _context.OfferSkills
                        .Count(skill => skillIds.Any(skillId =>
                            skill.Removed == null &&
                            skill.SkillId == skillId &&
                            skill.OfferTemplateId == ot.OfferTemplateId
                        ))
                    ).ThenByDescending(ot => ot.Created);
            }

            switch (orderBy)
            {
                case CompanyUserOfferTemplateOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Name)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Name)
                        .ThenByDescending(ot => ot.Created);
                case CompanyUserOfferTemplateOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Created)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Created)
                        .ThenByDescending(ot => ot.Created);
                case CompanyUserOfferTemplateOrderBy.OfferTemplateName:
                    return ascending
                        ? query.OrderBy(ot => ot.Name)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Name)
                        .ThenByDescending(ot => ot.Created);
                default: //OfferTemplatesOrderBy.OfferTemplateCreated:
                    return ascending
                        ? query.OrderBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Created);
            }
        }
    }
}
