﻿// Ignore Spelling: Jwt
using Domain.Features.People.DomainEvents.AdministrationEvents;
using Domain.Features.People.DomainEvents.AuthorizationEvents;
using Domain.Features.People.DomainEvents.BlockingEvents;
using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Features.People.Entities;
using Domain.Features.People.Exceptions;
using Domain.Features.People.ValueObjects.BirthDates;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Info;
using Domain.Features.People.ValueObjects.PhoneNumbers;
using Domain.Shared.CustomProviders;
using Domain.Shared.CustomProviders.StringProvider;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Emails;
using Domain.Shared.ValueObjects.Ids;

namespace Domain.Features.People.Aggregates
{
    public partial class Person : TemplateEntity<PersonId>
    {
        // Properties
        public AddressId? AddressId { get; private set; } = null;
        // Main Part
        public Email Login { get; private set; } = null!;
        public string Salt { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public bool HasTwoFactorAuthentication { get; private set; } = false;
        public bool IsStudent { get; private set; } = false;
        public bool IsIndividual { get; private set; } = false;
        public bool IsAdministrator { get; private set; } = false;
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; } = null;
        public DateTime? Blocked { get; private set; } = null;
        // Additional Part
        public string? Logo { get; private set; } = null;
        public string? Name { get; private set; } = null!;
        public string? Surname { get; private set; } = null!;
        public string? Description { get; private set; } = null!;
        public Email? ContactEmail { get; private set; } = null!;
        public PhoneNumber? ContactPhoneNumber { get; private set; } = null!;
        public BirthDate? BirthDate { get; private set; } = null;
        // Collections
        private readonly Dictionary<string, PersonUrl> _urls = [];
        public IReadOnlyDictionary<string, PersonUrl> Urls => _urls;

        private readonly Dictionary<SkillId, PersonSkill> _skills = [];
        public IReadOnlyDictionary<SkillId, PersonSkill> Skills => _skills;
        // Derived Data
        public bool IsNotCompleteProfile =>
            string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Surname) ||
            string.IsNullOrWhiteSpace(ContactEmail);
        public bool HasRemoved => Removed.HasValue;
        public bool HasBlocked => Blocked.HasValue;


        // Public Methods
        public void RaiseProfileCreatedEvent(Guid dbPersonId)
        {
            this.Id = dbPersonId;
            AddDomainEvent((PersonProfileCreatedEvent)(this));
        }

        public void RaiseProfileActivatedEvent()
        {
            AddDomainEvent((PersonProfileActivatedEvent)(this));
        }


        public void Remove()
        {
            if (!Removed.HasValue)
            {
                var now = CustomTimeProvider.Now;
                Removed = now;
                AddDomainEvent(PersonProfileRemovedEvent.Prepare(this, now));
            }
        }

        public void Restore()
        {
            if (Removed.HasValue)
            {
                Removed = null;
                AddDomainEvent((PersonProfileRestoredEvent)(this));
            }
        }


        public void Block(string message)
        {
            if (!HasBlocked)
            {
                Blocked = CustomTimeProvider.Now;
                AddDomainEvent(PersonBlockedEvent.Prepare(this, message));
            }
        }

        public void UnBlock()
        {
            if (HasBlocked)
            {
                Blocked = null;
                AddDomainEvent(PersonUnBlockedEvent.Prepare(this));
            }
        }

        public void GrandAdministrator()
        {
            if (!IsAdministrator)
            {
                IsAdministrator = true;
                AddDomainEvent(PersonAdministrationGrantEvent.Prepare(this));
            }
        }

        public void RevokeAdministrator()
        {
            if (IsAdministrator)
            {
                IsAdministrator = false;
                AddDomainEvent(PersonAdministrationRevokeEvent.Prepare(this));
            }
        }

        public void RaiseAuthorization2StageEvent(
            string urlSegment,
            string code,
            DateTime codeValidTo)
        {
            AddDomainEvent(PersonAuthorization2StageEvent.Prepare(
                this,
                urlSegment,
                code,
                codeValidTo));
        }

        public void RaiseAuthorizationInvalidEvent(string reason)
        {
            AddDomainEvent(PersonAuthorizationInvalid.Prepare(
                this,
                reason));
        }

        public void RaiseAuthorizationLoginInEvent(
            string jwt,
            string refreshToken,
            DateTime refreshTokenValidTo)
        {
            AddDomainEvent(PersonAuthorizationLoginInEvent.Prepare(
                this,
                jwt,
                refreshToken,
                refreshTokenValidTo));
        }

        public void RaiseAuthorizationLogOutEvent(
           string jwt,
           string refreshToken,
           DateTime refreshTokenValidTo)
        {
            AddDomainEvent(PersonAuthorizationLogOutEvent.Prepare(
                this,
                jwt,
                refreshToken,
                refreshTokenValidTo));
        }

        public void RaiseInitiateResetPasswordEvent()
        {
            AddDomainEvent(PersonProfileInitiateResetPasswordEvent.Prepare(
                this));
        }

        public void SetAuthenticationData(string salt, string password)
        {
            Salt = salt;
            Password = password;
            if (AllowedRegistrationEvents)
            {
                AddDomainEvent(PersonProfileResetPasswordEvent.Prepare(
                this,
                salt,
                password));
            }
        }

        // Private Methods
        private void SetLogin(string? login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new PersonException(Messages.Enitity_Person_EmptyLogin);
            }
            Login = new Email(login);
        }


        private void SetName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new PersonException(Messages.Enitity_Person_EmptyName);
            }
            Name = CustomStringProvider.NormalizeWhitespace(
                name,
                WhiteSpace.All);
        }

        private void SetSurname(string? surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new PersonException(Messages.Enitity_Person_EmptySurname);
            }
            Surname = CustomStringProvider.NormalizeWhitespace(
                surname,
                WhiteSpace.All);
        }

        private void SetDescription(string? description)
        {
            Description = CustomStringProvider.NormalizeWhitespace(
                description,
                WhiteSpace.AllExceptNewLine);
        }

        private void SetUrls(IEnumerable<PersonUrlInfo> items)
        {
            if (!_urls.Any())
            {
                foreach (var item in items)
                {
                    var personUrl = (PersonUrl)item;
                    _urls[personUrl.Value] = personUrl;
                }
            }
            else
            {
                var itemsDictionary = items.ToDictionary(
                    i => i.Value);

                var existingKeys = _urls.Keys.ToHashSet();
                var itemsKeys = itemsDictionary.Keys.ToHashSet();

                var intersectKeys = existingKeys.Intersect(itemsKeys);
                var removedKeys = existingKeys.Except(intersectKeys);
                var newKeys = itemsKeys.Except(intersectKeys);

                foreach (var key in removedKeys)
                {
                    _urls[key].Remove();
                }
                foreach (var key in newKeys)
                {
                    _urls[key] = (PersonUrl)itemsDictionary[key];
                }
            }
        }

        private void SetSkills(IEnumerable<PersonSkillInfo> items)
        {
            if (!_skills.Any())
            {
                foreach (var item in items)
                {
                    var personSkill = (PersonSkill)item;
                    _skills[personSkill.SkillId] = personSkill;
                }
            }
            else
            {
                var itemsDictionary = items.ToDictionary(
                    i => (SkillId)i.SkillId);

                var existingKeys = _skills.Keys.ToHashSet();
                var itemsKeys = itemsDictionary.Keys.ToHashSet();

                var intersectKeys = existingKeys.Intersect(itemsKeys);
                var removedKeys = existingKeys.Except(intersectKeys);
                var newKeys = itemsKeys.Except(intersectKeys);

                foreach (var key in removedKeys)
                {
                    _skills[key].Remove();
                }
                foreach (var key in newKeys)
                {
                    _skills[key] = (PersonSkill)itemsDictionary[key];
                }
            }
        }
    }
}
