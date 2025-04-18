﻿using AutoMapper;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.Shared.Templates.Repositories;
using DatabasePerson = UseCase.RelationalDatabase.Models.Person;
using DatabasePersonSkill = UseCase.RelationalDatabase.Models.PersonSkill;
using DatabasePersonUrl = UseCase.RelationalDatabase.Models.Url;
using DomainLogin = Domain.Shared.ValueObjects.Emails.Email;
using DomainPerson = Domain.Features.People.Aggregates.Person;
using DomainPersonId = Domain.Features.People.ValueObjects.Ids.PersonId;

namespace UseCase.Roles.Users.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;


        // Constructor
        public PersonRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                    HttpCode.Conflict);
            }

            var dbPerson = _mapper.Map<DatabasePerson>(item);
            await _context.People.AddAsync(dbPerson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RepositoryCreateSingleResponse.ValidResponse();
        }

        public async Task<RepositorySelectResponse<DomainPerson>> GetAsync(
            DomainLogin login,
            CancellationToken cancellationToken)
        {
            var loginValue = login.Value;
            var dbPerson = await _context.People
               .Where(person => person.Login == loginValue)
               .FirstOrDefaultAsync(cancellationToken);

            return PreparePersonGetResponse(dbPerson);
        }


        public async Task<RepositorySelectResponse<DomainPerson>> GetAsync(
            DomainPersonId id,
            CancellationToken cancellationToken)
        {
            var personIdValue = id.Value;
            var dbPerson = await _context.People
                .Where(person => person.PersonId == personIdValue)
                .FirstOrDefaultAsync(cancellationToken);

            return PreparePersonGetResponse(dbPerson);
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            DomainPerson item,
            CancellationToken cancellationToken)
        {
            var itemId = item.Id?.Value ?? throw new KeyNotFoundException();
            var itemLogin = item.Login.Value;
            var itemContactEmail = item.ContactEmail?.Value;
            var itemContactPhoneNumber = item.ContactPhoneNumber?.Value;

            Expression<Func<DatabasePerson, bool>> expression = person =>
                person.PersonId == itemId ||
                person.Login == itemLogin ||
                (
                    itemContactEmail == null ||
                    person.ContactEmail == itemContactEmail
                ) ||
                (
                    itemContactPhoneNumber == null ||
                    person.PhoneNum == itemContactPhoneNumber
                );

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
            UpdatePerson(dbPerson, item);
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

        private static void UpdatePerson(DatabasePerson database, DomainPerson domain)
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
                        SkillId = item.SkillId,
                        Created = item.Created,
                        Removed = item.Removed,
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
                        UrlTypeId = item.UrlTypeId,
                        Created = item.Created,
                        Removed = item.Removed,
                    });
                }
            }
        }

        // Private Non Static Methods
        private RepositorySelectResponse<DomainPerson> PreparePersonGetResponse(
            DatabasePerson? dbPerson)
        {
            if (dbPerson == null)
            {
                return InvalidSelect(HttpCode.NotFound);
            }
            if (dbPerson.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }

            var domainPerson = _mapper.Map<DomainPerson>(dbPerson);
            if (domainPerson.IsBlocked)
            {
                return InvalidSelect(HttpCode.Forbidden, Messages.Entity_Person_Account_Blocked);
            }

            return ValidSelect(domainPerson);
        }
    }
}
