using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Shared.DTOs.Responses.Companies.AutoMapperProfile
{
    public class CompaniesProfile : Profile
    {
        public CompaniesProfile()
        {
            CreateMap<Address, AddressResponseDto>()
                .ForMember(
                dto => dto.StateName,
                opt => opt.MapFrom(db => (db.Street == null ? null : db.Street.Name)))
                .ForMember(
                dto => dto.CityName,
                opt => opt.MapFrom(db => (db.City.Name)))
                .ForMember(
                dto => dto.StateId,
                opt => opt.MapFrom(db => (db.City.State.StateId)))
                .ForMember(
                dto => dto.StateName,
                opt => opt.MapFrom(db => (db.City.State.Name)))
                .ForMember(
                dto => dto.CountryId,
                opt => opt.MapFrom(db => (db.City.State.Country.CountryId)))
                .ForMember(
                dto => dto.CountryName,
                opt => opt.MapFrom(db => (db.City.State.Country.Name)));

            CreateMap<Company, CompanyDto>();

            CreateMap<Branch, BranchDto>();

            // OfferTemplate Part
            CreateMap<OfferTemplate, OfferTemplateDto>()
                .ForMember(
                dto => dto.Skills,
                opt => opt.MapFrom(db => db.OfferSkills));

            CreateMap<OfferSkill, OfferSkillDto>();

            // Offer Part
            CreateMap<Offer, OfferDto>()
                .ForMember(
                dto => dto.WorkModes,
                opt => opt.MapFrom(db => db.OfferWorkModes))
                .ForMember(
                dto => dto.EmploymentTypes,
                opt => opt.MapFrom(db => db.OfferEmploymentTypes));

            CreateMap<OfferWorkMode, WorkModeDto>()
                .ConvertUsing((src, dest, context) =>
                context.Mapper.Map<WorkModeDto>(src.WorkMode));

            CreateMap<OfferEmploymentType, EmploymentTypeDto>()
                .ConvertUsing((src, dest, context) =>
                context.Mapper.Map<EmploymentTypeDto>(src.EmploymentType));

        }
    }
}
