using AutoMapper;
using Domain.Shared.CustomProviders;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.DTOs.Responses.People;
using UseCase.Shared.DTOs.Responses.People.FullProfile;
using UseCase.Shared.Enums;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Shared.DTOs.Responses.AutoMapperProfile
{
    public class ResponsesProfile : Profile
    {
        public ResponsesProfile()
        {

            //CreateMap<Branch, CompanyUserBranchDto>();

            CreateMap<OfferSkill, OfferSkillDto>();

            CreateMap<OfferTemplate, OfferTemplateDto>()
                .ForMember(
                dto => dto.Skills,
                opt => opt.MapFrom(db => db.OfferSkills));

            CreateMap<ContractCondition, ContractConditionDto>()
                .ConstructUsing((db, context) => new ContractConditionDto
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

            CreateMap<Offer, OfferDto>()
                .ConstructUsing((db, context) =>
                {
                    var now = CustomTimeProvider.Now;
                    OfferStatus status = OfferStatus.Active;
                    if (db.PublicationStart > now)
                    {
                        status = OfferStatus.Pending;
                    }
                    if (db.PublicationEnd.HasValue && db.PublicationEnd <= now)
                    {
                        status = OfferStatus.Expired;
                    }

                    return new OfferDto
                    {
                        OfferId = db.OfferId,
                        PublicationStart = db.PublicationStart,
                        PublicationEnd = db.PublicationEnd,
                        EmploymentLength = db.EmploymentLength,
                        WebsiteUrl = db.WebsiteUrl,
                        Status = status,
                        Company = context.Mapper.Map<CompanyDto>(
                            db.OfferConnections
                            .First(oc => oc.Removed == null)
                            .OfferTemplate
                            .Company
                            ),
                        OfferTemplate = context.Mapper.Map<OfferTemplateDto>(
                            db.OfferConnections
                            .First(oc => oc.Removed == null)
                            .OfferTemplate
                            ),
                        Branch = db.Branch == null
                            ? null
                            : context.Mapper.Map<CompanyUserBranchDto>(db.Branch),
                        ContractConditions = db.OfferConditions
                            .Select(x => context.Mapper.Map<ContractConditionDto>(x.ContractCondition)),
                    };
                });


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
