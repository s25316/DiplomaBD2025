using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Users.Queries.GetPersonRecruitments.Request;
using UseCase.Roles.Users.Queries.GetPersonRecruitments.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.ExtensionMethods.EF.Recruitments;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.BaseResponses.Guest;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitments
{
    public class GetPersonRecruitmentsHandler : IRequestHandler<GetPersonRecruitmentsRequest, ItemsResponse<UserRecruitmentDataDto>>
    {
        // Properties 
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor 
        public GetPersonRecruitmentsHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<UserRecruitmentDataDto>> Handle(GetPersonRecruitmentsRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var baseQuery = PrepareQuery(personId, request);
            var paginatedQuery = baseQuery.Paginate(request.Pagination);

            var selectResult = await paginatedQuery.Select(recruitment => new
            {
                Recruitment = recruitment,
                TotalCount = baseQuery.Count(),
                OfferTemplate = _context.OfferConnections
                    .Include(oc => oc.OfferTemplate)
                    .ThenInclude(ot => ot.Company)
                    .Where(oc =>
                        oc.Removed == null &&
                        oc.OfferId == recruitment.OfferId)
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
                            .ToList()
                    }).First(),
                ContractConditions = _context.OfferConditions
                    .Include(oc => oc.ContractCondition)
                    .Where(oc =>
                    oc.Removed == null &&
                        oc.OfferId == recruitment.OfferId)
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
                Person = _context.People
                    .Include(p => p.Address)
                    .ThenInclude(p => p.City)
                    .ThenInclude(p => p.State)
                    .ThenInclude(p => p.Country)
                    .Where(p => p.PersonId == personId.Value)
                    .Select(person => new
                    {
                        Person = person,
                        Sklills = _context.PersonSkills
                            .Include(x => x.Skill)
                            .ThenInclude(x => x.SkillType)
                            .Where(ps =>
                                ps.PersonId == person.PersonId &&
                                ps.Removed == null
                            ).ToList(),
                        Urls = _context.Urls
                            .Include(x => x.UrlType)
                            .Where(url =>
                                url.PersonId == person.PersonId &&
                                url.Removed == null)
                            .ToList()
                    })
                    .First(),
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);


            // Prepare Response
            var first = selectResult.FirstOrDefault();
            if (!selectResult.Any() || first == null)
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            // Person
            var dbPerson = first.Person.Person;
            dbPerson.Urls = first.Person.Urls;
            dbPerson.PersonSkills = first.Person.Sklills;
            // Total Count
            var totalCount = first.TotalCount;

            var items = new List<UserRecruitmentDataDto>();
            foreach (var item in selectResult)
            {
                var company = item.OfferTemplate.Item.Company;
                if (company.Removed.HasValue)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }

                if (company.Blocked.HasValue)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }
                if (item.Recruitment.PersonId != personId.Value)
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


                items.Add(new UserRecruitmentDataDto
                {
                    Person = _mapper.Map<CompanyUserPersonProfile>(dbPerson),
                    Recruitment = _mapper.Map<RecruitmentDto>(item.Recruitment),
                    Offer = _mapper.Map<OfferDto>(item.Recruitment.Offer),
                    Branch = _mapper.Map<GuestBranchDto>(item.Recruitment.Offer.Branch),
                    Company = _mapper.Map<CompanyDto>(company),
                    OfferTemplate = _mapper.Map<GuestOfferTemplateDto>(item.OfferTemplate.Item),
                    ContractConditions = _mapper.Map<IEnumerable<GuestContractConditionDto>>(contractConditions),
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<UserRecruitmentDataDto> PrepareResponse(
            HttpCode code,
            IEnumerable<UserRecruitmentDataDto> items,
            int totalCount)
        {
            return ItemsResponse<UserRecruitmentDataDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(GetPersonRecruitmentsRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private IQueryable<HrProcess> PrepareBaseQuery()
        {
            return _context.HrProcesses
                .Include(r => r.ProcessType)

                .Include(o => o.Offer)
                .ThenInclude(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.Street)

                .Include(o => o.Offer)
                .ThenInclude(o => o.Branch)
                .ThenInclude(oc => oc.Address)
                .ThenInclude(oc => oc.City)
                .ThenInclude(oc => oc.State)
                .ThenInclude(oc => oc.Country)
                .AsNoTracking();
        }

        private IQueryable<HrProcess> PrepareQuery(
            PersonId personId,
            GetPersonRecruitmentsRequest request)
        {
            var personIdValue = personId.Value;
            var query = PrepareBaseQuery();

            if (request.RecruitmentId.HasValue)
            {
                return query.WhereRecruitmentId(request.RecruitmentId.Value);
            }
            if (request.CompanyQueryParameters.HasValue)
            {
                query = query.WhereCompanyIdentificationData(
                    _context,
                    null,
                    request.CompanyQueryParameters);
            }
            query = query
                .Where(recruitment => recruitment.PersonId == personIdValue)
                .Where(recruitment => _context.Companies
                    .Include(c => c.OfferTemplates)
                    .ThenInclude(oc => oc.OfferConnections)
                    .Where(company =>
                        company.Removed == null &&
                        company.Blocked == null)
                    .Any(company =>
                        company.OfferTemplates.Any(ot =>
                            ot.OfferConnections.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId
                            ))
                    ));

            query = query.WhereSalary(_context, request.SalaryParameters)
                .WhereContractParameters(_context, request.ContractParameterIds)
                .WhereSkills(_context, request.SkillIds)
                .WhereText(_context, request.SearchText)
                .WhereOfferParameters(_context, request.OfferQueryParameters)
                .WhereProcessType(request.ProcessType);

            query = request.Ascending
                ? query.OrderBy(recruitment => recruitment.Created)
                : query.OrderByDescending(recruitment => recruitment.Created);
            return query;
        }
    }
}
