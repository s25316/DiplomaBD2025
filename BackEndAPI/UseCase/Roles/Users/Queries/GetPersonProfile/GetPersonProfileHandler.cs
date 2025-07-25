﻿using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Users.Queries.GetPersonProfile.Request;
using UseCase.Roles.Users.Queries.GetPersonProfile.Response;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.BaseResponses.User;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Queries.GetPersonProfile
{
    public class GetPersonProfileHandler : IRequestHandler<GetPersonProfileRequest, GetPersonProfileResponse>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GetPersonProfileHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetPersonProfileResponse> Handle(GetPersonProfileRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var dbPersonData = await _context.People
                .Include(p => p.Address)
                .ThenInclude(p => p.City)
                .ThenInclude(p => p.State)
                .ThenInclude(p => p.Country)

                .Include(p => p.Address)
                .ThenInclude(p => p.Street)
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
                .AsNoTracking()
                .FirstAsync(cancellationToken);


            var dbPerson = dbPersonData.Person;
            dbPerson.Urls = dbPersonData.Urls;
            dbPerson.PersonSkills = dbPersonData.Sklills;

            return new GetPersonProfileResponse
            {
                PersonPerspective = _mapper.Map<UserPersonProfile>(dbPerson),
                CompanyPerspective = _mapper.Map<CompanyUserPersonProfile>(dbPerson),
            };
        }

        // Non Static Methods
        private PersonId GetPersonId(GetPersonProfileRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
