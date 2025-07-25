﻿using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.BaseEFRepository;
using DatabasePerson = UseCase.RelationalDatabase.Models.Person;
using DatabasePersonSkill = UseCase.RelationalDatabase.Models.PersonSkill;
using DatabasePersonUrl = UseCase.RelationalDatabase.Models.Url;
using DomainLogin = Domain.Shared.ValueObjects.Emails.Email;
using DomainPerson = Domain.Features.People.Aggregates.Person;
using DomainPersonId = Domain.Features.People.ValueObjects.Ids.PersonId;

namespace UseCase.Shared.Repositories.People
{
    public class PersonRepository : IPersonRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly DiplomaBdContext _context;


        // Constructor
        public PersonRepository(
            IMapper mapper,
            IMediator mediator,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }


        // Public methods
        public async Task<RepositoryCreateSingleResponse> CreateAsync(
            DomainPerson item,
            CancellationToken cancellationToken)
        {
            var itemLogin = item.Login.Value;
            var duplicatesCount = await _context.People
                .Where(person => person.Login == itemLogin)
                .CountAsync(cancellationToken);
            if (duplicatesCount != 0)
            {
                return RepositoryCreateSingleResponse.InvalidResponse(
                    HttpCode.Conflict,
                    Messages.Entity_Person_Login_Duplicate);
            }

            var dbPerson = _mapper.Map<DatabasePerson>(item);
            await _context.People.AddAsync(dbPerson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            item.RaiseProfileCreatedEvent(dbPerson.PersonId);
            await PublishEventsAsync(item, cancellationToken);

            return RepositoryCreateSingleResponse.ValidResponse();
        }

        public async Task<RepositorySelectResponse<DomainPerson>> GetAsync(
            DomainLogin login,
            CancellationToken cancellationToken)
        {
            var loginValue = login.Value;
            var selectResult = await _context.People
               .Where(person => person.Login == loginValue)
               .Select(person => new
               {
                   Person = person,
                   Urls = _context.Urls
                        .Where(url =>
                            url.PersonId == person.PersonId &&
                            url.Removed == null)
                        .ToList(),
                   Skills = _context.PersonSkills
                        .Where(skill =>
                            skill.PersonId == person.PersonId &&
                            skill.Removed == null)
                        .ToList(),
               })
                .FirstAsync(cancellationToken);

            selectResult.Person.PersonSkills = selectResult.Skills;
            selectResult.Person.Urls = selectResult.Urls;

            return PreparePersonGetResponse(selectResult.Person);
        }


        public async Task<RepositorySelectResponse<DomainPerson>> GetAsync(
            DomainPersonId id,
            CancellationToken cancellationToken)
        {
            var personIdValue = id.Value;
            var selectResult = await _context.People
                .Where(person => person.PersonId == personIdValue)
                .Select(person => new
                {
                    Person = person,
                    Urls = _context.Urls
                        .Where(url =>
                            url.PersonId == person.PersonId &&
                            url.Removed == null)
                        .ToList(),
                    Skills = _context.PersonSkills
                        .Where(skill =>
                            skill.PersonId == person.PersonId &&
                            skill.Removed == null)
                        .ToList(),
                })
                .FirstAsync(cancellationToken);

            selectResult.Person.PersonSkills = selectResult.Skills;
            selectResult.Person.Urls = selectResult.Urls;

            return PreparePersonGetResponse(selectResult.Person);
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            DomainPerson item,
            CancellationToken cancellationToken)
        {
            var itemId = item.Id?.Value
                ?? throw new UseCaseLayerException();
            var itemLogin = item.Login.Value;
            var itemContactEmail = item.ContactEmail?.Value;
            var itemContactPhoneNumber = item.ContactPhoneNumber?.Value;

            Expression<Func<DatabasePerson, bool>> expression = person =>
                person.PersonId == itemId ||
                person.Login == itemLogin ||

                    itemContactEmail != null &&
                    person.ContactEmail == itemContactEmail
                 ||

                    itemContactPhoneNumber != null &&
                    person.PhoneNum == itemContactPhoneNumber
                ;

            var people = await _context.People
                .Include(p => p.Urls)
                .Include(p => p.PersonSkills)
                .Where(expression)
                .ToListAsync(cancellationToken);

            if (!people.Any())
            {
                return InvalidUpdate(HttpCode.NotFound);
            }

            if (people.Count > 1)
            {
                var stringBuilder = new StringBuilder();
                foreach (var dbItem in people)
                {
                    if (dbItem.PersonId == itemId)
                    {
                        continue;
                    }

                    if (itemLogin == dbItem.Login)
                    {
                        stringBuilder.AppendLine(
                        Messages.Entity_Person_Login_Duplicate);
                    }
                    if (!string.IsNullOrWhiteSpace(itemContactEmail) &&
                        itemContactEmail == dbItem.ContactEmail)
                    {
                        stringBuilder.AppendLine(
                            Messages.Entity_Person_ContactEmail_Duplicate);
                    }
                    if (!string.IsNullOrWhiteSpace(itemContactPhoneNumber) &&
                        itemContactPhoneNumber == dbItem.PhoneNum)
                    {
                        stringBuilder.AppendLine(
                        Messages.Entity_Person_ContactPhoneNumber_Duplicate);
                    }
                }
                return InvalidUpdate(
                    HttpCode.Conflict,
                    stringBuilder.ToString());
            }

            var dbPerson = people[0];
            await UpdatePerson(dbPerson, item, cancellationToken);


            await PublishEventsAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ValidUpdate();
        }

        public async Task<RepositoryRemoveResponse> RemoveAsync(
            DomainPerson item,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Private Static Methods
        private static RepositorySelectResponse<DomainPerson> ValidSelect(
           DomainPerson item)
        {
            return RepositorySelectResponse<DomainPerson>.ValidResponse(item);
        }

        private static RepositorySelectResponse<DomainPerson> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainPerson>
                .InvalidResponse(code, message);
        }

        private static RepositoryUpdateResponse ValidUpdate()
        {
            return RepositoryUpdateResponse.ValidResponse();
        }

        private static RepositoryUpdateResponse InvalidUpdate(
            HttpCode code,
            string? message = null)
        {
            return RepositoryUpdateResponse.InvalidResponse(code, message);
        }

        private async Task UpdatePerson(DatabasePerson database, DomainPerson domain, CancellationToken cancellationToken)
        {
            database.AddressId = domain.AddressId?.Value;
            database.Login = domain.Login.Value;
            database.Logo = domain.Logo;
            database.Name = domain.Name;
            database.Surname = domain.Surname;
            database.Description = domain.Description;
            database.PhoneNum = domain.ContactPhoneNumber?.Value;
            database.ContactEmail = domain.ContactEmail?.Value;
            database.BirthDate = domain.BirthDate?.Value;
            database.IsTwoFactorAuth = domain.HasTwoFactorAuthentication;
            database.IsStudent = domain.IsStudent;
            database.IsAdmin = domain.IsAdministrator;
            database.Salt = domain.Salt;
            database.Password = domain.Password;
            database.Created = domain.Created;
            database.Blocked = domain.Blocked;
            database.Removed = domain.Removed;
            database.IsIndividual = domain.IsIndividual;

            var skillsDictionary = database.PersonSkills
                .Where(s => s.Removed == null)
                .ToDictionary(
                s => s.PersonSkillId);

            var newSkills = new List<DatabasePersonSkill>();
            foreach (var item in domain.Skills.Values)
            {
                if (item.Id != null)
                {
                    var dbSkill = skillsDictionary[item.Id.Value];
                    dbSkill.Removed = item.Removed;
                }
                else
                {
                    newSkills.Add(new DatabasePersonSkill
                    {
                        Person = database,
                        SkillId = item.SkillId,
                        Created = item.Created,
                    });
                }
            }

            var urlsDictionary = database.Urls
                .Where(s => s.Removed == null)
                .ToDictionary(
                s => s.UrlId);

            var newUrls = new List<DatabasePersonUrl>();
            foreach (var item in domain.Urls.Values)
            {
                if (item.Id != null)
                {
                    var dbUrl = urlsDictionary[item.Id.Value];
                    dbUrl.Removed = item.Removed;
                }
                else
                {
                    newUrls.Add(new DatabasePersonUrl
                    {
                        Person = database,
                        UrlTypeId = item.UrlTypeId,
                        Value = item.Value,
                        Created = item.Created,
                    });
                }
            }

            await _context.Urls.AddRangeAsync(newUrls, cancellationToken);
            await _context.PersonSkills.AddRangeAsync(newSkills, cancellationToken);
        }

        // Private Non Static Methods
        private async Task PublishEventsAsync(DomainPerson domain, CancellationToken cancellationToken)
        {
            foreach (var @event in domain.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            domain.ClearEvents();
        }

        private RepositorySelectResponse<DomainPerson> PreparePersonGetResponse(
            DatabasePerson? dbPerson)
        {
            if (dbPerson == null)
            {
                return InvalidSelect(HttpCode.NotFound);
            }
            if (string.IsNullOrWhiteSpace(dbPerson.Login))
            {
                return InvalidSelect(HttpCode.Gone);
            }

            var domainPerson = _mapper.Map<DomainPerson>(dbPerson);
            if (domainPerson.HasBlocked)
            {
                return InvalidSelect(HttpCode.Forbidden, Messages.Entity_Person_Account_Blocked);
            }

            return ValidSelect(domainPerson);
        }
    }
}
