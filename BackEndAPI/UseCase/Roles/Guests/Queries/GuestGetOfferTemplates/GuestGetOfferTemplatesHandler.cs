using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Request;
using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.Offers;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.Guest;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetOfferTemplates
{
    public class GuestGetOfferTemplatesHandler : IRequestHandler<GuestGetOfferTemplatesRequest, ItemsResponse<GuestOfferTemplateAndCompanyDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        //private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GuestGetOfferTemplatesHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<GuestOfferTemplateAndCompanyDto>> Handle(GuestGetOfferTemplatesRequest request, CancellationToken cancellationToken)
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
                Skills = _context.OfferSkills
                    .Include(os => os.Skill)
                    .ThenInclude(os => os.SkillType)
                    .Where(os =>
                    os.Removed == null &&
                    os.OfferTemplateId == item.OfferTemplateId)
                    .ToList(),
                OfferCount = _context.Offers
                    .Include(offer => offer.OfferConnections)
                    .Where(offer => offer.OfferConnections.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferConnectionId == item.OfferTemplateId))
                    .Count(getActiveOffersExpression),

            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.NotFound, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<GuestOfferTemplateAndCompanyDto>();
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

                item.Item.OfferSkills = item.Skills;
                items.Add(new GuestOfferTemplateAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    OfferTemplate = _mapper.Map<GuestOfferTemplateDto>(item.Item),
                    OfferCount = item.OfferCount,
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<GuestOfferTemplateAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<GuestOfferTemplateAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<GuestOfferTemplateAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private IQueryable<OfferTemplate> PrepareBaseQuery()
        {
            return _context.OfferTemplates
                .Include(ot => ot.Company)
                .AsNoTracking();
        }

        private IQueryable<OfferTemplate> PrepareQuery(
            GuestGetOfferTemplatesRequest request,
            Expression<Func<Offer, bool>> getActiveOffers)
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
                query = query.Where(ot =>
                    ot.Company.Removed == null &&
                    ot.Company.Blocked == null)
                    .Where(ot =>
                    ot.Removed == null ||
                    (
                        ot.Removed != null &&
                        _context.Offers
                        .Where(offer => offer.OfferConnections.Any(oc =>
                            oc.Removed == null &&
                            oc.OfferTemplateId == ot.OfferTemplateId
                        ))
                        .Any(getActiveOffers)
                    ));
            }

            query = query.WhereText(request.SearchText);
            query = WhereSkills(query, request.SkillIds);

            return OrderBy(
                query,
                request.SkillIds,
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
            GuestOfferTemplateOrderBy orderBy,
            bool ascending)
        {
            if (skillIds.Any() &&
                orderBy == GuestOfferTemplateOrderBy.Skills)
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
                case GuestOfferTemplateOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Name)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Name)
                        .ThenByDescending(ot => ot.Created);
                case GuestOfferTemplateOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Created)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Created)
                        .ThenByDescending(ot => ot.Created);
                case GuestOfferTemplateOrderBy.OfferTemplateName:
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
