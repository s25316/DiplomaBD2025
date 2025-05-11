using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders.StringProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Response;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetOfferTemplates
{
    public class GetCompanyUserOfferTemplatesHandler :
        GetCompanyUserGenericsBase<OfferTemplate, CompanyAndOfferTemplateDto>,
        IRequestHandler<GetCompanyUserOfferTemplatesRequest, GetCompanyUserGenericItemsResponse<CompanyAndOfferTemplateDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserOfferTemplatesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector
            ) : base(context, mapper, authenticationInspector)
        { }


        // Methods
        public async Task<GetCompanyUserGenericItemsResponse<CompanyAndOfferTemplateDto>> Handle(GetCompanyUserOfferTemplatesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = BuildQuery(personId, request);

            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                offerTemplate => offerTemplate.Company.CompanyPeople);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectedValues,
                offerTemplate => offerTemplate.Company.Removed != null,
                offerTemplate => new CompanyAndOfferTemplateDto
                {
                    Company = _mapper.Map<CompanyDto>(offerTemplate.Company),
                    OfferTemplate = _mapper.Map<OfferTemplateDto>(offerTemplate),
                });
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

        // Private Non Static Methods        
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
            ///Tu problklem !!!
            var query = BuildBaseQuery();

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
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters)
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
            var searchWords = CustomStringProvider
                .Split(request.SearchText, WhiteSpace.All);

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
    }
}
