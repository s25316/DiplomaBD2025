using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.People.Aggregates
{
    public partial class Person : TemplateEntity<PersonId>
    {
        public class Updater : TemplateUpdater<Person, PersonId>
        {
            // Constructor
            public Updater(Person value) : base(value)
            {
            }


            // Public Methods
            public Updater SetAddressId(Guid? addressId)
            {
                SetProperty(person => person.AddressId = addressId);
                return this;
            }

            public Updater SetLogin(string login)
            {
                SetProperty(person => person.SetLogin(login));
                return this;
            }

            public Updater SetAuthenticationData(string salt, string password)
            {
                SetProperty(person => person.SetAuthenticationData(salt, password));
                return this;
            }

            public Updater SetHasTwoFactorAuthentication(bool isTwo)
            {
                SetProperty(person => person.HasTwoFactorAuthentication = isTwo);
                return this;
            }

            public Updater SetIsStudent(bool isStudent)
            {
                SetProperty(person => person.IsStudent = isStudent);
                return this;
            }

            public Updater SetIsAdministrator(bool isAdministrator)
            {
                SetProperty(person => person.IsAdministrator = isAdministrator);
                return this;
            }

            public Updater SetLogo(string? logo)
            {
                SetProperty(person => person.Logo = logo);
                return this;
            }

            public Updater SetName(string name)
            {
                SetProperty(person => person.SetName(name));
                return this;
            }

            public Updater SetSurname(string surname)
            {
                SetProperty(person => person.SetSurname(surname));
                return this;
            }

            public Updater SetDescription(string? description)
            {
                SetProperty(person => person.SetDescription(description));
                return this;
            }

            public Updater SetContactEmail(string contactEmail)
            {
                SetProperty(person => person.ContactEmail = contactEmail);
                return this;
            }

            public Updater SetContactPhoneNumber(string contactPhoneNumber)
            {
                SetProperty(person => person.ContactPhoneNumber = contactPhoneNumber);
                return this;
            }

            public Updater SetBirthDate(DateOnly? birthDate)
            {
                SetProperty(person => person.BirthDate = birthDate);
                return this;
            }

            public Updater SetSkills(IEnumerable<PersonSkillInfo> items)
            {
                SetProperty(person => person.SetSkills(items));
                return this;
            }

            public Updater SetUrls(IEnumerable<PersonUrlInfo> items)
            {
                SetProperty(person => person.SetUrls(items));
                return this;
            }

            // Protected Methods
            protected override Func<Person, string> CheckIsObjectComplete()
            {
                return person =>
                {
                    var stringBuilder = new StringBuilder();
                    if (person.Login == null)
                    {
                        stringBuilder.AppendLine("Empty Login");
                    }
                    if (string.IsNullOrWhiteSpace(person.Salt) ||
                        string.IsNullOrWhiteSpace(person.Password))
                    {
                        stringBuilder.AppendLine("Empty Authentication Data");
                    }
                    return stringBuilder.ToString().Trim();
                };
            }

            protected override Action<Person> SetDefaultValues()
            {
                return person =>
                {
                    if (person.Created == DateTime.MinValue)
                    {
                        person.Created = CustomTimeProvider.Now;
                    }

                };
            }
        }
    }
}
