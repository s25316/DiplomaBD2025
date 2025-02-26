// Ignore Spelling: Automapper

using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.AutomapperProfile
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

            CreateMap<Skill, SkillResponseDto>().ForMember(
                dest => dest.SkillType,
                opt => opt.MapFrom(src => src.SkillType)
                );
        }
    }
}
