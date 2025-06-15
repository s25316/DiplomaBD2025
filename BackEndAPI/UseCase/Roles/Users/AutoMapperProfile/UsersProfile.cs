using AutoMapper;
using Domain.Features.People.ValueObjects.Info;
using DatabasePerson = UseCase.RelationalDatabase.Models.Person;
using DomianPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.AutoMapperProfile
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<DomianPerson, DatabasePerson>()
                .ConstructUsing(domain => new DatabasePerson
                {
                    Login = domain.Login.Value,
                    AddressId = domain.AddressId != null
                        ? domain.AddressId.Value
                        : null,
                    Salt = domain.Salt,
                    Password = domain.Password,
                    IsTwoFactorAuth = domain.HasTwoFactorAuthentication,
                    IsStudent = domain.IsStudent,
                    IsAdmin = domain.IsAdministrator,
                    Created = domain.Created,
                    Removed = domain.Removed,
                    Blocked = domain.Blocked,
                    Logo = domain.Logo,
                    Name = domain.Logo,
                    Surname = domain.Logo,
                    Description = domain.Logo,
                    ContactEmail = domain.ContactEmail != null
                        ? domain.ContactEmail.Value
                        : null,
                    PhoneNum = domain.ContactPhoneNumber != null
                        ? domain.ContactPhoneNumber.Value
                        : null,
                    BirthDate = domain.BirthDate != null
                        ? domain.BirthDate.Value
                        : null,
                })
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<DatabasePerson, DomianPerson>()
               .ConstructUsing(db => new DomianPerson.Builder()
                    .SetId(db.PersonId)
                    .SetAddressId(db.AddressId)
                    .SetLogin(db.Login ?? "")
                    .SetAuthenticationData(db.Salt, db.Password)
                    .SetHasTwoFactorAuthentication(db.IsTwoFactorAuth)
                    .SetIsStudent(db.IsStudent)
                    .SetIsIndividual(db.IsIndividual)
                    .SetIsAdministrator(db.IsAdmin)
                    .SetCreated(db.Created)
                    .SetRemoved(db.Removed)
                    .SetBlocked(db.Blocked)
                    .SetLogo(db.Logo)
                    .SetName(db.Name)
                    .SetSurname(db.Surname)
                    .SetDescription(db.Description)
                    .SetContactEmail(db.ContactEmail)
                    .SetContactPhoneNumber(db.PhoneNum)
                    .SetBirthDate(db.BirthDate)
                    .SetSkills(db.PersonSkills
                        .Where(s => s.Removed == null)
                        .Select(s => new PersonSkillInfo
                        {
                            Id = s.PersonSkillId,
                            SkillId = s.SkillId,
                            Created = s.Created,
                            Removed = s.Removed,
                        }))
                    .SetUrls(db.Urls
                        .Where(url => url.Removed == null)
                        .Select(url => new PersonUrlInfo
                        {
                            Id = url.UrlId,
                            Value = url.Value ?? "",
                            UrlTypeId = url.UrlTypeId,
                            Created = url.Created,
                            Removed = url.Removed,
                        }))
                    .Build())
               .ForAllMembers(opt => opt.Ignore());
        }
    }
}
