// Ignore Spelling: Jwt
using Domain.Features.People.DomainEvents;
using Domain.Features.People.DomainEvents.AuthorizationEvents;
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
        private readonly Dictionary<int, PersonUrl> _urls = [];
        public IReadOnlyDictionary<int, PersonUrl> Urls => _urls;

        private readonly Dictionary<SkillId, PersonSkill> _skills = [];
        public IReadOnlyDictionary<SkillId, PersonSkill> Skills => _skills;
        // Derived Data
        public bool IsCompleteProfile =>
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
            if (Removed.HasValue)
            {
                Removed = null;
            }
            else
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public void Block()
        {
            if (Blocked.HasValue)
            {
                Blocked = null;
            }
            else
            {
                Blocked = CustomTimeProvider.Now;
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

        // Private Methods
        private void SetLogin(string? login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new PersonException(Messages.Enitity_Person_EmptyLogin);
            }
            Login = new Email(login);
        }

        private void SetAuthenticationData(string salt, string password)
        {
            Salt = salt;
            Password = password;
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
                    _urls[personUrl.UrlTypeId] = personUrl;
                }
            }
            else
            {
                var itemsDictionary = items.ToDictionary(
                    i => i.UrlTypeId);

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
            if (!_urls.Any())
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
                    _urls[key].Remove();
                }
                foreach (var key in newKeys)
                {
                    _skills[key] = (PersonSkill)itemsDictionary[key];
                }
            }
        }
    }
}
