using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Shared.Responses.BaseResponses.Guest.AutoMapperProfile
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<Branch, GuestBranchDto>();

            CreateMap<OfferTemplate, GuestOfferTemplateDto>()
                .ForMember(
                dto => dto.Skills,
                opt => opt.MapFrom(db => db.OfferSkills));

            CreateMap<ContractCondition, GuestContractConditionDto>()
                .ConstructUsing((db, context) => new GuestContractConditionDto
                {
                    ContractConditionId = db.ContractConditionId,
                    CompanyId = db.CompanyId,
                    HoursPerTerm = db.HoursPerTerm,
                    SalaryMin = db.SalaryMin ?? 0,
                    SalaryMax = db.SalaryMax ?? 0,
                    SalaryAvg = ((db.SalaryMin ?? 0) + (db.SalaryMax ?? 0)) / 2,
                    SalaryPerHourAvg = ((((db.SalaryMin ?? 0) + (db.SalaryMax ?? 0)) / 2) / db.HoursPerTerm),
                    SalaryPerHourMin = (db.SalaryMin ?? 0) / db.HoursPerTerm,
                    SalaryPerHourMax = (db.SalaryMax ?? 0) / db.HoursPerTerm,
                    IsPaid = (db.SalaryMin ?? 0) > 0,
                    IsNegotiable = db.IsNegotiable,
                    Created = db.Created,
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
