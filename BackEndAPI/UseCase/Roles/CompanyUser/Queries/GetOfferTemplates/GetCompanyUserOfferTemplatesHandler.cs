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
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetOfferTemplates
{
    public class GetCompanyUserOfferTemplatesHandler : IRequestHandler<GetCompanyUserOfferTemplatesRequest, GetCompanyUserOfferTemplatesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserOfferTemplatesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyUserOfferTemplatesResponse> Handle(GetCompanyUserOfferTemplatesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = BuildQuery(personId, request);
            var selector = BuldSelector(personId, query);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(selectedValues);
        }

        // Private static Methods
        private sealed class SelectResult
        {
            public required OfferTemplate OfferTemplate { get; init; }
            public required int AuthorizedRolesCount { get; init; }
            public required int TotalCount { get; init; }
        }

        private static Expression<Func<OfferTemplate, SelectResult>> BuldSelector(
            PersonId personId,
            IQueryable<OfferTemplate> totalCountQuery)
        {
            return ot => new SelectResult
            {
                OfferTemplate = ot,
                AuthorizedRolesCount = ot.Company.CompanyPeople.Count(role =>
                    _authorizedRoles.Any(roleId =>
                            role.PersonId == personId.Value &&
                            role.RoleId == (int)roleId &&
                            role.Deny == null
                    )),
                TotalCount = totalCountQuery.Count(),
            };
        }

        private static IQueryable<OfferTemplate> ApplyOerderBy(
            IQueryable<OfferTemplate> query,
            CompanyUserOfferTemplatesOrderBy orderBy,
            bool ascending,
            bool showRemoved,
            IEnumerable<int> skillIds)
        {
            if (showRemoved &&
                orderBy == CompanyUserOfferTemplatesOrderBy.OfferTemplateRemoved)
            {
                return ascending
                        ? query.OrderBy(ot => ot.Removed)
                        : query.OrderByDescending(ot => ot.Removed);
            }

            if (skillIds.Any() &&
                orderBy == CompanyUserOfferTemplatesOrderBy.Skills)
            {
                return ascending
                    ? query.OrderBy(ot => ot.OfferSkills
                        .Count(skill => skillIds.Any(skillId =>
                            skill.SkillId == skillId &&
                            skill.Removed == null
                        ))
                    ).ThenBy(ot => ot.Created)
                    : query.OrderByDescending(ot => ot.OfferSkills
                        .Count(skill => skillIds.Any(skillId =>
                            skill.SkillId == skillId &&
                            skill.Removed == null
                        ))
                    ).ThenByDescending(ot => ot.Created);
            }

            switch (orderBy)
            {
                case CompanyUserOfferTemplatesOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Name)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Name)
                        .ThenByDescending(ot => ot.Created);
                case CompanyUserOfferTemplatesOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Created)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Created)
                        .ThenByDescending(ot => ot.Created);
                case CompanyUserOfferTemplatesOrderBy.OfferTemplateName:
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

        private static GetCompanyUserOfferTemplatesResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserOfferTemplatesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndOfferTemplateDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserOfferTemplatesResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyAndOfferTemplateDto> dtos,
            int totalCount)
        {
            return new GetCompanyUserOfferTemplatesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndOfferTemplateDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(GetCompanyUserOfferTemplatesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private IQueryable<OfferTemplate> BuildBaseQuery()
        {
            return _context.OfferTemplates

                .Include(ot => ot.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(ot => ot.OfferSkills)
                .ThenInclude(os => os.Skill)
                .ThenInclude(s => s.SkillType)
                .AsNoTracking();
        }

        private IQueryable<OfferTemplate> BuildQuery(
            PersonId personId,
            GetCompanyUserOfferTemplatesRequest request)
        {
            var query = BuildBaseQuery()
                .Where(ot => ot.OfferSkills.Any(os =>
                    os.Removed == null
                ));

            // For Single OfferTemplate
            if (request.OfferTemplateId.HasValue)
            {
                return query
                    .Where(ot => ot.OfferTemplateId == request.OfferTemplateId.Value);
            }

            // Filter Companies if we haven`t access to it 
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                query = query.Where(ot => _context.Companies
                    .IdentificationFilter(personId, request.CompanyParameters)
                    .Any(company => company.CompanyId == ot.CompanyId));
            }
            // Filter Companies to which we have access and eliminate Removed
            else
            {
                query = query
                    .Where(ot => ot.Company.Removed == null)
                    .Where(ot => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == ot.CompanyId));
            }

            // Show Removed Part
            query = query.Where(ot => request.ShowRemoved
                ? ot.Removed != null
                : ot.Removed == null);

            // Search text Part
            var searchWords = CustomStringProvider.Split(request.SearchText);
            if (searchWords.Any())
            {
                query = query.SearchTextFilter(searchWords);
            }

            // Skills Part
            if (request.SkillIds.Any())
            {
                query = query.SkillsFilter(request.SkillIds);
            }

            // Apply Sorting
            query = ApplyOerderBy(
                query,
                request.OrderBy,
                request.Ascending,
                request.ShowRemoved,
                request.SkillIds);
            return query;
        }

        private GetCompanyUserOfferTemplatesResponse PrepareResponse(List<SelectResult> items)
        {
            if (!items.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<CompanyAndOfferTemplateDto>();
            for (int i = 0; i < items.Count; i++)
            {
                var selectedValue = items[i];
                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                if (selectedValue.AuthorizedRolesCount == 0)
                {
                    return InvalidResponse(HttpCode.Forbidden);
                }
                if (selectedValue.OfferTemplate.Company.Removed != null)
                {
                    return InvalidResponse(HttpCode.Gone);
                }
                dtos.Add(new CompanyAndOfferTemplateDto
                {
                    Company = _mapper.Map<CompanyDto>(selectedValue.OfferTemplate.Company),
                    OfferTemplate = _mapper.Map<OfferTemplateDto>(selectedValue.OfferTemplate),
                });
            }
            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }
    }
}
