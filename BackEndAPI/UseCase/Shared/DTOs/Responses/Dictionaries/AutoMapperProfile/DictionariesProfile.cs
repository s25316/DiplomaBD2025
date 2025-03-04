﻿// Ignore Spelling: 

using AutoMapper;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.DTOs.Responses.Dictionaries.AutoMapperProfile
{
    public class DictionariesProfile : Profile
    {
        public DictionariesProfile()
        {
            CreateMap<SalaryTerm, SalaryTermDto>();
            CreateMap<SalaryTermDto, SalaryTerm>();

            CreateMap<WorkMode, WorkModeDto>();
            CreateMap<WorkModeDto, WorkMode>();

            CreateMap<EmploymentType, EmploymentTypeDto>();
            CreateMap<EmploymentTypeDto, EmploymentType>();

            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, Currency>();

            CreateMap<Role, CompanyRoleDto>();
            CreateMap<CompanyRoleDto, Role>();

            CreateMap<SkillType, SkillTypeDto>();
            CreateMap<SkillTypeDto, SkillType>();

            CreateMap<NotificationType, NotificationTypeDto>();
            CreateMap<NotificationTypeDto, NotificationType>();

            CreateMap<Faq, FaqDto>();
            CreateMap<FaqDto, Faq>();

            CreateMap<UrlType, UrlTypeDto>();
            CreateMap<UrlTypeDto, UrlType>();

            CreateMap<Skill, SkillResponseDto>()
                .ForMember(
                dest => dest.SkillType,
                opt => opt.MapFrom(src => src.SkillType)
                );
        }
    }
}
