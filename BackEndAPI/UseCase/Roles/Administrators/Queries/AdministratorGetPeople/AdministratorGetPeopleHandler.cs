using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Administrators.Queries.AdministratorGetPeople.Request;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.People;
using UseCase.Shared.Responses.BaseResponses.Administrator;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetPeople
{
    public class AdministratorGetPeopleHandler : IRequestHandler<AdministratorGetPeopleRequest, ItemsResponse<AdministratorPersonProfile>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorGetPeopleHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<AdministratorPersonProfile>> Handle(AdministratorGetPeopleRequest request, CancellationToken cancellationToken)
        {
            var baseQuery = PrepareQuery(request);
            var paginatedQuery = baseQuery.Paginate(request.PaginationQueryParameters);

            var items = await paginatedQuery
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
                        .ToList(),
                    TotalCount = baseQuery.Count(),
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);


            if (!items.Any())
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;
            var dtos = new List<AdministratorPersonProfile>();
            foreach (var dbPersonData in items)
            {
                var dbPerson = dbPersonData.Person;
                dbPerson.Urls = dbPersonData.Urls;
                dbPerson.PersonSkills = dbPersonData.Sklills;
                dtos.Add(_mapper.Map<AdministratorPersonProfile>(dbPerson));
            }

            return PrepareResponse(HttpCode.Ok, dtos, totalCount);
        }

        // Static Methods
        private static ItemsResponse<AdministratorPersonProfile> PrepareResponse(
            HttpCode code,
            IEnumerable<AdministratorPersonProfile> items,
            int totalCount)
        {
            return ItemsResponse<AdministratorPersonProfile>.PrepareResponse(code, items, totalCount);
        }

        // Non Static methods
        private IQueryable<Person> BaseQuery()
        {
            return _context.People
                .Include(p => p.Address)
                .ThenInclude(p => p.City)
                .ThenInclude(p => p.State)
                .ThenInclude(p => p.Country)

                .Include(p => p.Address)
                .ThenInclude(p => p.Street)
                .AsNoTracking();
        }

        private IQueryable<Person> PrepareQuery(AdministratorGetPeopleRequest request)
        {
            var query = BaseQuery();
            if (request.PersonId.HasValue)
            {
                return query
                    .Where(person => person.PersonId == request.PersonId);
            }

            query = query
                .WhereEmail(request.Email)
                .WhereSearchText(request.SearchText)
                .WhereRemoved(request.ShowRemoved);


            return request.Ascending
                ? query.OrderBy(person => person.Created)
                : query.OrderByDescending(person => person.Created);
        }
    }
}
