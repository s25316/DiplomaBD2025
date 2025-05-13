using AutoMapper;
using Domain.Shared.CustomProviders;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;
using UseCase.Shared.DTOs.Responses.People;
using UseCase.Shared.DTOs.Responses.People.FullProfile;
using UseCase.Shared.Responses.BaseResponses;

namespace UseCase.Shared.DTOs.Responses.AutoMapperProfile
{
    public class ResponsesProfile : Profile
    {
        public ResponsesProfile()
        {
            CreateMap<Skill, SkillDto>()
                .ConstructUsing(db => new SkillDto
                {
                    SkillId = db.SkillId,
                    Name = db.Name,
                    SkillType = new Dictionaries.GetSkillTypes.Response.SkillTypeDto
                    {
                        SkillTypeId = db.SkillType.SkillTypeId,
                        Name = db.SkillType.Name,
                    },
                });

            CreateMap<Url, UrlDto>()
                .ConstructUsing(db => new UrlDto
                {
                    Value = db.Value ?? "",
                    UrlType = new UrlTypeDto
                    {
                        UrlTypeId = db.UrlType.UrlTypeId,
                        Name = db.UrlType.Name,
                    }
                });

            CreateMap<Person, FullPersonProfile>()
                .ConstructUsing((db, context) => new FullPersonProfile
                {
                    Logo = db.Logo,
                    Name = db.Name,
                    Surname = db.Surname,
                    Description = db.Description,
                    PhoneNum = db.PhoneNum,
                    ContactEmail = db.ContactEmail,
                    BirthDate = db.BirthDate == null ? null : CustomTimeProvider.GetDateTime(db.BirthDate.Value),
                    IsTwoFactorAuth = db.IsTwoFactorAuth,
                    IsStudent = db.IsStudent,
                    IsAdmin = db.IsAdmin,
                    Created = db.Created,
                    Blocked = db.Blocked,
                    Removed = db.Removed,
                    Address = context.Mapper.Map<AddressResponseDto>(db.Address),
                    Skills = db.PersonSkills
                        .Where(x => x.Removed == null)
                        .Select(skill => context.Mapper.Map<SkillDto>(skill.Skill)),
                    Urls = db.Urls
                        .Where(x => x.Removed == null)
                        .Select(x => context.Mapper.Map<UrlDto>(x)),
                })
                .ForAllMembers(x => x.Ignore());

        }
    }
}
