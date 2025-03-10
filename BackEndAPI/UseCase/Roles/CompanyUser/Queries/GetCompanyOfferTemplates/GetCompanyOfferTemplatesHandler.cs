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
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.Enums;
using UseCase.Shared.ExtensionMethods;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates
{
    class GetCompanyOfferTemplatesHandler : IRequestHandler<GetCompanyOfferTemplatesRequest, GetCompanyOfferTemplatesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyOfferTemplatesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyOfferTemplatesResponse> Handle(GetCompanyOfferTemplatesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var query = BuildQuery(personId, request);
            var selector = BuldSelector(personId, query);

            var selectedValues = await query
                .Paginate(request.Page, request.ItemsPerPage)
                .Select(selector)
                .ToListAsync(cancellationToken);

            var totalCount = -1;
            var isForbidden = false;
            var isRemoved = false;
            var dtos = new List<CompanyAndOfferTemplate>();
            for (int i = 0; i < selectedValues.Count; i++)
            {
                var selectedValue = selectedValues[i];
                if (totalCount < 0)
                {
                    totalCount = selectedValue.TotalCount;
                }
                if (selectedValue.AuthorizedRolesCount == 0)
                {
                    isForbidden = true;
                    break;
                }
                if (selectedValue.OfferTemplate.Company.Removed != null)
                {
                    isRemoved = true;
                    break;
                }
                dtos.Add(new CompanyAndOfferTemplate
                {
                    Company = _mapper.Map<CompanyDto>(selectedValue.OfferTemplate.Company),
                    OfferTemplate = _mapper.Map<OfferTemplateDto>(selectedValue.OfferTemplate),
                });
            }

            if (!selectedValues.Any())
            {
                return InvalidResponse(HttpCode.NotFound);
            }
            if (isForbidden)
            {
                return InvalidResponse(HttpCode.Forbidden);
            }
            if (isRemoved)
            {
                return InvalidResponse(HttpCode.Gone);
            }
            return ValidResponse(HttpCode.Ok, dtos, totalCount);
            throw new NotImplementedException();
        }

        // Private Methods
        private PersonId GetPersonId(GetCompanyOfferTemplatesRequest request)
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
            GetCompanyOfferTemplatesRequest request)
        {
            var query = BuildBaseQuery();

            if (request.OfferTemplateId.HasValue)
            {
                var filter = BuildOfferTemplateFilter(request.OfferTemplateId.Value);
                query = query.Where(filter);
            }
            else
            {
                if (request.CompanyId.HasValue ||
                    request.Regon != null ||
                    request.Nip != null ||
                    request.Krs != null)
                {
                    var filter = BuildCompanyFilter(
                        request.CompanyId,
                        request.Regon,
                        request.Nip,
                        request.Krs);
                    query = query.Where(filter);
                }
                else
                {
                    query = query.Where(ot => ot.Company.CompanyPeople.Any(role =>
                        _authorizedRoles.Any(roleId =>
                            role.PersonId == personId.Value &&
                            role.RoleId == (int)roleId &&
                            role.Deny == null
                    )));
                }

                var otherFilters = BuildOtherFilters(
                    request.SearchText,
                    request.SkillIds,
                    request.ShowRemoved);
                query = query.Where(otherFilters);
                query = ApplyOerderBy(
                    query,
                    request.OrderBy,
                    request.Ascending,
                    request.ShowRemoved,
                    request.SkillIds);
            }
            return query;
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

        private static Expression<Func<OfferTemplate, bool>> BuildOfferTemplateFilter(
            Guid offerTemplateId)
        {
            return ot => ot.OfferTemplateId == offerTemplateId;
        }

        private static Expression<Func<OfferTemplate, bool>> BuildCompanyFilter(
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs)
        {
            if (companyId.HasValue)
            {
                return ot => ot.Company.CompanyId == companyId.Value;
            }

            return ot =>
                (regon == null || ot.Company.Regon == regon) &&
                (nip == null || ot.Company.Nip == nip) &&
                (krs == null || ot.Company.Krs == krs);
        }

        private static Expression<Func<OfferTemplate, bool>> BuildOtherFilters(
            string? searchText,
            IEnumerable<int> skillIds,
            bool showRemoved)
        {
            var searchWords = CustomStringProvider.Split(searchText);

            return ot =>
                (
                    !searchWords.Any() || searchWords.Any(word =>
                    ot.Name.Contains(word) ||
                    ot.Description.Contains(word) ||
                    (ot.Company.Name != null && ot.Company.Name.Contains(word)) ||
                    (ot.Company.Description != null && ot.Company.Description.Contains(word))
                )) &&
                (
                    showRemoved
                        ? ot.Removed != null
                        : ot.Removed == null
                ) &&
                (
                    !skillIds.Any() || skillIds.Any(skillId =>
                        ot.OfferSkills.Any(skill =>
                            skill.SkillId == skillId &&
                            skill.Removed == null
                )));
        }

        private static IQueryable<OfferTemplate> ApplyOerderBy(
            IQueryable<OfferTemplate> query,
            OfferTemplatesOrderBy orderBy,
            bool ascending,
            bool showRemoved,
            IEnumerable<int> skillIds)
        {
            if (showRemoved &&
                orderBy == OfferTemplatesOrderBy.OfferTemplateRemoved)
            {
                return ascending
                        ? query.OrderBy(ot => ot.Removed)
                        : query.OrderByDescending(ot => ot.Removed);
            }

            if (skillIds.Any() &&
                orderBy == OfferTemplatesOrderBy.Skills)
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
                case OfferTemplatesOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Name)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Name)
                        .ThenByDescending(ot => ot.Created);
                case OfferTemplatesOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(ot => ot.Company.Created)
                        .ThenBy(ot => ot.Created)
                        : query.OrderByDescending(ot => ot.Company.Created)
                        .ThenByDescending(ot => ot.Created);
                case OfferTemplatesOrderBy.OfferTemplateName:
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

        private static GetCompanyOfferTemplatesResponse InvalidResponse(HttpCode code)
        {
            return new GetCompanyOfferTemplatesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndOfferTemplate>
                {
                    Items = [],
                    TotalCount = 0,
                },
                IsCorrect = false,
                HttpCode = code,
            };
        }

        private static GetCompanyOfferTemplatesResponse ValidResponse(
            HttpCode code,
            IEnumerable<CompanyAndOfferTemplate> dtos,
            int totalCount)
        {
            return new GetCompanyOfferTemplatesResponse
            {
                Result = new ResponseQueryResultTemplate<CompanyAndOfferTemplate>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                },
                IsCorrect = true,
                HttpCode = code,
            };
        }
    }
}
