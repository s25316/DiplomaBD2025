using AutoMapper;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainBranch = Domain.Features.Branches.Entities.Branch;
using DomainCompany = Domain.Features.Companies.Entities.Company;
using DomainOffer = Domain.Features.Offers.Entities.Offer;
using DomainOfferSkill = Domain.Features.OfferTemplates.ValueObjects.OfferSkill; // have no pair
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;
namespace UseCase.Roles.CompanyUser.Commands.AutoMapperProfile
{
    public class CompanyUserCommandsProfile : Profile
    {
        public CompanyUserCommandsProfile()
        {
            CreateMap<DomainCompany, DatabaseCompany>()
                .ForMember(
                desc => desc.CompanyId,
                opt => opt.Ignore());
            CreateMap<DatabaseCompany, DomainCompany>()
                .ConstructUsing(src => new DomainCompany.Builder()
                    .SetId(src.CompanyId)
                    .SetName(src.Name)
                    .SetDescription(src.Description)
                    .SetRegon(src.Regon)
                    .SetNip(src.Nip)
                    .SetKrs(src.Krs)
                    .SetWebsiteUrl(src.WebsiteUrl)
                    .SetCreated(src.Created)
                    .SetRemoved(src.Removed)
                    .SetBlocked(src.Blocked)
                    .Build());

            CreateMap<DomainBranch, DatabaseBranch>()
                .ForMember(
                desc => desc.BranchId,
                opt => opt.Ignore())
                .ForMember(desc => desc.CompanyId,
                opt => opt.MapFrom(src => src.CompanyId.Value))
                .ForMember(
                desc => desc.AddressId,
                opt => opt.MapFrom(src => src.AddressId.Value));
            CreateMap<DatabaseBranch, DomainBranch>()
                .ConstructUsing(src => new DomainBranch.Builder()
                    .SetId(src.BranchId)
                    .SetAddressId(src.AddressId)
                    .SetCompanyId(src.CompanyId)
                    .SetName(src.Name)
                    .SetDescription(src.Description)
                    .SetCreated(src.Created)
                    .SetRemoved(src.Removed)
                    .Build());


            CreateMap<DomainOfferTemplate, DatabaseOfferTemplate>()
                .ForMember(
                desc => desc.OfferTemplateId,
                opt => opt.Ignore());
            CreateMap<DatabaseOfferTemplate, DomainOfferTemplate>()
                .ConstructUsing(src => new DomainOfferTemplate.Builder()
                    .SetId(src.OfferTemplateId)
                    .SetCompanyId(src.CompanyId)
                    .SetName(src.Name)
                    .SetDescription(src.Description)
                    .SetCreated(src.Created)
                    .SetRemoved(src.Removed)
                    .SetSkillsIds(src.OfferSkills.Select(od => new DomainOfferSkill
                    {
                        IsRequired = od.IsRequired,
                        SkillId = od.SkillId
                    }))
                    .Build());

            CreateMap<DomainOffer, DatabaseOffer>()
                .ConstructUsing(domain => new DatabaseOffer
                {
                    // OfferId not inputed 
                    OfferTemplateId = domain.OfferTemplateId.Value,
                    BranchId = domain.BranchId,
                    PublicationStart = domain.PublicationRange.Start,
                    PublicationEnd = domain.PublicationRange.End,
                    WorkBeginDate = domain.WorkRange == null ? null : domain.WorkRange.Start,
                    WorkEndDate = domain.WorkRange == null ? null : domain.WorkRange.End,
                    SalaryRangeMin = domain.SalaryRange.Min,
                    SalaryRangeMax = domain.SalaryRange.Max,
                    SalaryTermId = domain.SalaryTermId,
                    CurrencyId = domain.CurrencyId,
                    IsNegotiated = domain.IsNegotiated,
                    WebsiteUrl = domain.WebsiteUrl,
                });
            CreateMap<DatabaseOffer, DomainOffer>()
                .ConstructUsing(database => new DomainOffer.Builder()
                    .SetId(database.OfferId)
                    .SetOfferTemplateId(database.OfferTemplateId)
                    .SetBranchId(database.BranchId)
                    .SetDatesRanges(
                        database.PublicationStart,
                        database.PublicationEnd,
                        database.WorkBeginDate,
                        database.WorkEndDate)
                    .SetSalaryData(
                        database.SalaryRangeMin,
                        database.SalaryRangeMax,
                        database.SalaryTermId,
                        database.CurrencyId,
                        database.IsNegotiated)
                    .SetWorkModeIds(database.OfferWorkModes
                        .Where(wm => wm.Removed == null)
                        .Select(wm => wm.WorkModeId))
                    .SetEmploymentTypeIds(database.OfferEmploymentTypes
                        .Where(et => et.Removed == null)
                        .Select(et => et.EmploymentTypeId))
                    .SetWebsiteUrl(database.WebsiteUrl)
                    .Build());
        }
    }
}
