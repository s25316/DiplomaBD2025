using Domain.Features.People.Aggregates;
using Domain.Features.People.DomainEvents.AdministrationEvents;
using Domain.Features.People.DomainEvents.AuthorizationEvents;
using Domain.Features.People.DomainEvents.BlockingEvents;
using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Features.People.ValueObjects.Info;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class AggregatePersonTests
    {
        // Properties
        public static IEnumerable<object[]> TestData_Person_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    "surname",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
            }
        }

        // Methods
        public static IEnumerable<object[]> TestData_PersonBuilder_Invalid_TemplateBuilderException
        {
            get
            {
                yield return new object[]
                {
                    (string?) null,
                    (string?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    "login",
                    (string?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    "login",
                    "salt",
                    (string?) null,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_PersonBuilder_Invalid_TemplateBuilderException))]
        public void PersonBuilder_Invalid_TemplateBuilderException(
            string? login,
            string? salt,
            string? password)
        {
            var builder = new Person.Builder();

            if (!string.IsNullOrWhiteSpace(salt) &&
                !string.IsNullOrWhiteSpace(password))
            {
                builder.SetAuthenticationData(salt, password);
            }

            var result = Assert.Throws<TemplateBuilderException>(
                () => builder.Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        public static IEnumerable<object[]> TestData_PersonBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    (Guid?) null,
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    (DateTime?) null,
                    (DateTime?) null,
                    (DateTime?) null,
                    (string?) null,
                    "name",
                    "surname",
                    (string?) null,
                    (string?) null,
                    (string?) null,
                    (DateOnly?) null,
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    "surname",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_PersonBuilder_Correct_Correct))]
        public void PersonBuilder_Correct_Correct(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.Equal(skillIds.Count(), item.Skills.Count);
            Assert.Equal(urls.Count(), item.Urls.Count);
            Assert.Equal(isAdministrator, item.IsAdministrator);
            Assert.Equal(blocked.HasValue, item.HasBlocked);
            Assert.Equal(removed.HasValue, item.HasRemoved);
            if (!string.IsNullOrWhiteSpace(contactEmail))
            {
                Assert.False(item.IsNotCompleteProfile);
            }
        }


        public static IEnumerable<object[]> TestData_PersonBuilder_InCorrect_HasErrors
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    " ",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    "surname",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    " ",
                    "surname",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    " ",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_PersonBuilder_InCorrect_HasErrors))]
        public void PersonBuilder_InCorrect_HasErrors(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.True(builder.HasErrors());
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseProfileActivatedEvent_PersonProfileCreatedEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseProfileActivatedEvent();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileActivatedEvent>(item.DomainEvents.First());
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RestoreRemove_PersonProfileRestoredEventPersonProfileRemovedEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.True(item.Removed.HasValue);
            item.Restore();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileRestoredEvent>(item.DomainEvents.First());
            item.ClearEvents();

            Assert.False(item.Removed.HasValue);
            item.Restore();
            Assert.Equal(0, item.DomainEvents.Count);


            item.Remove();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileRemovedEvent>(item.DomainEvents.First());
            item.ClearEvents();

            item.Remove();
            Assert.Equal(0, item.DomainEvents.Count);
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_UnBlockBlock_PersonUnBlockedEventPersonBlockedEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.True(item.HasBlocked);
            item.UnBlock();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonUnBlockedEvent>(item.DomainEvents.First());
            item.ClearEvents();

            Assert.False(item.HasBlocked);
            item.UnBlock();
            Assert.Equal(0, item.DomainEvents.Count);


            item.Block("");
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonBlockedEvent>(item.DomainEvents.First());
            item.ClearEvents();

            item.Block("");
            Assert.Equal(0, item.DomainEvents.Count);
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_GrandRevokeAdministrator_PersonAdministrationGrantEventPersonAdministrationRevokeEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.False(item.IsAdministrator);
            item.GrandAdministrator();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAdministrationGrantEvent>(item.DomainEvents.First());
            item.ClearEvents();

            Assert.True(item.HasBlocked);
            item.GrandAdministrator();
            Assert.Equal(0, item.DomainEvents.Count);


            item.RevokeAdministrator();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAdministrationRevokeEvent>(item.DomainEvents.First());
            item.ClearEvents();


            item.RevokeAdministrator();
            Assert.Equal(0, item.DomainEvents.Count);
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseAuthorization2StageEvent_PersonAuthorization2StageEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseAuthorization2StageEvent("", "", DateTime.Now);
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAuthorization2StageEvent>(item.DomainEvents.First());
            item.ClearEvents();
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseAuthorizationInvalidEvent_PersonAuthorizationInvalid(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseAuthorizationInvalidEvent("");
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAuthorizationInvalid>(item.DomainEvents.First());
            item.ClearEvents();
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseAuthorizationLoginInEvent_PersonAuthorizationLoginInEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseAuthorizationLoginInEvent("", "", DateTime.Now);
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAuthorizationLoginInEvent>(item.DomainEvents.First());
            item.ClearEvents();
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseAuthorizationLogOutEvent_PersonAuthorizationLogOutEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseAuthorizationLogOutEvent("", "", DateTime.Now);
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonAuthorizationLogOutEvent>(item.DomainEvents.First());
            item.ClearEvents();
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseInitiateResetPasswordEvent_PersonProfileInitiateResetPasswordEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseInitiateResetPasswordEvent();
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileInitiateResetPasswordEvent>(item.DomainEvents.First());
            item.ClearEvents();
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_RaiseProfileCreatedEvent_PersonProfileCreatedEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.RaiseProfileCreatedEvent(id);
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileCreatedEvent>(item.DomainEvents.First());
            item.ClearEvents();

            Assert.Equal(id, item.Id.Value);
        }


        [Theory]
        [MemberData(nameof(TestData_Person_Correct))]
        public void PersonBuilder_SetAuthenticationData_PersonProfileResetPasswordEvent(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            item.SetAuthenticationData(salt, password);
            Assert.Equal(1, item.DomainEvents.Count);
            Assert.IsType<PersonProfileResetPasswordEvent>(item.DomainEvents.First());
            item.ClearEvents();
        }

        public static IEnumerable<object[]> TestData_PersonUpdater_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    Guid.NewGuid(), //Address2
                    "a@gmail.com",
                    "ab@gmail.com",
                    "salt",
                    "Password",
                    false, //2Factor
                    true, // 2Factor2
                    false, // IsStudent
                    true, // IsStudent2
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    "name2",
                    "surname",
                    "surname2",
                    "description",
                    "description2",
                    "a@gmail.com",
                    "ab@gmail.com",
                    "123456789",
                    "123456798",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-19)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<int>)[2,3,4,5],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ]
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData_PersonUpdater_Correct_Correct))]
        public void PersonUpdater_Correct_Correct(
            Guid id,
            Guid? addressId,
            Guid? addressId2,
            string login,
            string login2,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isTwoStepAuthorization2,
            bool isStudent,
            bool isStudent2,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string name2,
            string surname,
            string surname2,
            string? description,
            string? description2,
            string? contactEmail,
            string? contactEmail2,
            string? phoneNumber,
            string? phoneNumber2,
            DateOnly? birthDate,
            DateOnly? birthDate2,
            IEnumerable<int> skillIds,
            IEnumerable<int> skillIds2,
            IEnumerable<(int id, string url)> urls)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Person.Updater(item)
                .SetAddressId(addressId2)
                .SetLogin(login2)
                .SetIsStudent(isStudent2)
                .SetName(name2)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization2)
                .SetSurname(surname2)
                .SetDescription(description2)
                .SetContactEmail(contactEmail2)
                .SetContactPhoneNumber(phoneNumber2)
                .SetBirthDate(birthDate2)
                .SetSkills(skillIds2.Select(skillId => (PersonSkillInfo)skillId));

            Assert.False(updater.HasErrors());
            var item2 = updater.Build();

            Assert.Equal(addressId2, item2.AddressId.Value);
            Assert.Equal(login2, item2.Login.Value);

            var excepted = skillIds.Except(skillIds2).Count();
            Assert.Equal(
                excepted,
                item2.Skills.Values.Where(skill => skill.Removed.HasValue).Count());
        }

        public static IEnumerable<object[]> TestData_PersonUpdater_UpdateUrls_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(), //Address
                    "a@gmail.com",
                    "salt",
                    "Password",
                    false,
                    false,
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "logo",
                    "name",
                    "surname",
                    "description",
                    "a@gmail.com",
                    "123456789",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://www.wp.pl/"),
                        ],
                    (IEnumerable<(int id, string url)>)[
                        (1, "https://www.youtube.com/"),
                        (1, "https://github.com/"),
                        ]
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData_PersonUpdater_UpdateUrls_Correct))]
        public void PersonUpdater_UpdateUrls_Correct(
            Guid id,
            Guid? addressId,
            string login,
            string salt,
            string password,
            bool isTwoStepAuthorization,
            bool isStudent,
            bool isAdministrator,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked,
            string? logo,
            string name,
            string surname,
            string? description,
            string? contactEmail,
            string? phoneNumber,
            DateOnly? birthDate,
            IEnumerable<int> skillIds,
            IEnumerable<(int id, string url)> urls,
            IEnumerable<(int id, string url)> urls2)
        {
            var builder = new Person.Builder()
                .SetId(id)
                .SetAddressId(addressId)
                .SetLogin(login)
                .SetAuthenticationData(salt, password)
                .SetHasTwoFactorAuthentication(isTwoStepAuthorization)
                .SetIsStudent(isStudent)
                .SetIsAdministrator(isAdministrator)
                .SetRemoved(removed)
                .SetBlocked(blocked)
                .SetLogo(logo)
                .SetName(name)
                .SetSurname(surname)
                .SetDescription(description)
                .SetContactEmail(contactEmail)
                .SetContactPhoneNumber(phoneNumber)
                .SetBirthDate(birthDate)
                .SetSkills(skillIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(urls.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Person.Updater(item)
                .SetUrls(urls2.Select(x => new PersonUrlInfo
                {
                    Id = null,
                    Value = x.url,
                    UrlTypeId = x.id,
                    Created = null,
                    Removed = null
                }));

            Assert.False(updater.HasErrors());
            var item2 = updater.Build();

            Assert.Equal(
                1,
                item.Urls.Values.Where(x => x.Removed.HasValue).Count());

            Assert.Equal(
                3,
                item.Urls.Values.Count());
        }
    }
}
