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
        public class Builder : TemplateBuilder<Person>
        {
            // Public Methods
            public Builder SetAddressId(Guid? addressId)
            {
                SetProperty(person => person.AddressId = addressId);
                return this;
            }

            public Builder SetLogin(string login)
            {
                SetProperty(person => person.SetLogin(login));
                return this;
            }

            public Builder SetAuthenticationData(string salt, string password)
            {
                SetProperty(person => person.SetAuthenticationData(salt, password));
                return this;
            }

            public Builder SetHasTwoFactorAuthentication(bool isTwo)
            {
                SetProperty(person => person.HasTwoFactorAuthentication = isTwo);
                return this;
            }

            public Builder SetIsStudent(bool isStudent)
            {
                SetProperty(person => person.IsStudent = isStudent);
                return this;
            }

            public Builder SetIsAdministrator(bool isAdministrator)
            {
                SetProperty(person => person.IsAdministrator = isAdministrator);
                return this;
            }

            public Builder SetCreated(DateTime created)
            {
                SetProperty(person => person.Created = created);
                return this;
            }

            public Builder SetRemoved(DateTime? removed)
            {
                SetProperty(person => person.Removed = removed);
                return this;
            }

            public Builder SetBlocked(DateTime? blocked)
            {
                SetProperty(person => person.Blocked = blocked);
                return this;
            }

            public Builder SetLogo(string? logo)
            {
                SetProperty(person => person.Logo = logo);
                return this;
            }

            public Builder SetName(string name)
            {
                SetProperty(person => person.SetName(name));
                return this;
            }

            public Builder SetSurname(string surname)
            {
                SetProperty(person => person.SetSurname(surname));
                return this;
            }

            public Builder SetDescription(string? description)
            {
                SetProperty(person => person.SetDescription(description));
                return this;
            }

            public Builder SetContactEmail(string contactEmail)
            {
                SetProperty(person => person.ContactEmail = contactEmail);
                return this;
            }

            public Builder SetContactPhoneNumber(string contactPhoneNumber)
            {
                SetProperty(person => person.ContactPhoneNumber = contactPhoneNumber);
                return this;
            }

            public Builder SetBirthDate(DateOnly? birthDate)
            {
                SetProperty(person => person.BirthDate = birthDate);
                return this;
            }

            public Builder SetSkills(IEnumerable<PersonSkillInfo> items)
            {
                SetProperty(person => person.SetSkills(items));
                return this;
            }

            public Builder SetUrls(IEnumerable<PersonUrlInfo> items)
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