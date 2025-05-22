using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Enums;

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser.AutoMapperProfile
{
    public class CompanyUserProfile : Profile
    {
        public CompanyUserProfile()
        {
            CreateMap<Branch, CompanyUserBranchDto>();

            CreateMap<OfferTemplate, CompanyUserOfferTemplateDto>()
                .ForMember(
                dto => dto.Skills,
                opt => opt.MapFrom(db => db.OfferSkills));

            CreateMap<ContractCondition, CompanyUserContractConditionDto>()
                .ConstructUsing((db, context) => new CompanyUserContractConditionDto
                {
                    ContractConditionId = db.ContractConditionId,
                    CompanyId = db.CompanyId,
                    HoursPerTerm = db.HoursPerTerm,
                    SalaryMin = db.SalaryMin,
                    SalaryMax = db.SalaryMax,
                    SalaryAvg = ((db.SalaryMin) + (db.SalaryMax)) / 2,
                    SalaryPerHourAvg = ((((db.SalaryMin) + (db.SalaryMax)) / 2) / db.HoursPerTerm),
                    SalaryPerHourMin = (db.SalaryMin) / db.HoursPerTerm,
                    SalaryPerHourMax = (db.SalaryMax) / db.HoursPerTerm,
                    IsPaid = (db.SalaryMin) > 0,
                    IsNegotiable = db.IsNegotiable,
                    Created = db.Created,
                    Removed = db.Removed,
                    SalaryTerm = context.Mapper.Map<ContractParameterDto>(db.ContractAttributes
                        .Where(x => x.Removed == null && x.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.SalaryTerm)
                        .OrderByDescending(x => x.Created)
                        .Select(x => x.ContractParameter)
                        .FirstOrDefault()),
                    Currency = context.Mapper.Map<ContractParameterDto>(db.ContractAttributes
                        .Where(x => x.Removed == null && x.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.Currency)
                        .OrderByDescending(x => x.Created)
                        .Select(x => x.ContractParameter)
                        .FirstOrDefault()),
                    WorkModes = context.Mapper.Map<IEnumerable<ContractParameterDto>>(db.ContractAttributes
                        .Where(x => x.Removed == null && x.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.WorkMode)
                        .OrderByDescending(x => x.Created)
                        .Select(x => x.ContractParameter)
                        .AsEnumerable()),
                    EmploymentTypes = context.Mapper.Map<IEnumerable<ContractParameterDto>>(db.ContractAttributes
                        .Where(x => x.Removed == null && x.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.EmploymentType)
                        .OrderByDescending(x => x.Created)
                        .Select(x => x.ContractParameter)
                        .AsEnumerable()),
                });
        }
    }
}
