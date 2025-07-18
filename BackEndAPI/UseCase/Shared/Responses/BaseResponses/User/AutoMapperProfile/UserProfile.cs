﻿using AutoMapper;
using Domain.Shared.CustomProviders;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.Responses.BaseResponses.User.AutoMapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Person, UserPersonProfile>()
                .ConstructUsing((db, context) => new UserPersonProfile
                {
                    IsIndividual = db.IsIndividual,
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
