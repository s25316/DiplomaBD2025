using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;
using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.GetSkillTypes.Response;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;

namespace UseCase.Shared.Dictionaries.AutoMapperProfile
{
    public class DictionariesProfile : Profile
    {
        public DictionariesProfile()
        {
            CreateMap<ContractParameter, ContractParameterDto>();

            CreateMap<ContractParameterType, ContractParameterTypeDto>();

            CreateMap<Faq, FaqDto>();

            CreateMap<Skill, SkillDto>();

            CreateMap<SkillType, SkillTypeDto>();

            CreateMap<UrlType, UrlTypeDto>();

            CreateMap<ProcessType, ProcessTypeDto>();
        }
    }
}
