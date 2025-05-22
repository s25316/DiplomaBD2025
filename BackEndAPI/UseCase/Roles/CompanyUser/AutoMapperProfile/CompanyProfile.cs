using AutoMapper;
using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.OfferTemplates.ValueObjects.Info;
using UseCase.Shared.Enums;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DatabaseContractCondition = UseCase.RelationalDatabase.Models.ContractCondition;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainBranch = Domain.Features.Branches.Entities.Branch;
using DomainCompany = Domain.Features.Companies.Entities.Company;
using DomainContractCondition = Domain.Features.ContractConditions.Aggregates.ContractCondition;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;

namespace UseCase.Roles.CompanyUser.AutoMapperProfile
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            // Company Mapping
            CreateMap<DomainCompany, DatabaseCompany>()
                .ForMember(
                database => database.CompanyId,
                opt => opt.Ignore());

            CreateMap<DatabaseCompany, DomainCompany>()
                .ConstructUsing(db => new DomainCompany.Builder()
                    .SetId(db.CompanyId)
                    .SetLogo(db.Logo)
                    .SetName(db.Name)
                    .SetDescription(db.Description)
                    .SetRegon(db.Regon)
                    .SetNip(db.Nip)
                    .SetKrs(db.Krs)
                    .SetWebsiteUrl(db.WebsiteUrl)
                    .SetCreated(db.Created)
                    .SetRemoved(db.Removed)
                    .SetBlocked(db.Blocked)
                    .Build())
                .ForAllMembers(opt => opt.Ignore());

            // Branch Mapping
            CreateMap<DomainBranch, DatabaseBranch>()
                .ForMember(
                database => database.BranchId,
                opt => opt.Ignore());

            CreateMap<DatabaseBranch, DomainBranch>()
                .ConstructUsing(db => new DomainBranch.Builder()
                    .SetId(db.BranchId)
                    .SetCompanyId(db.CompanyId)
                    .SetAddressId(db.AddressId)
                    .SetName(db.Name)
                    .SetDescription(db.Description)
                    .SetCreated(db.Created)
                    .SetRemoved(db.Removed)
                    .Build())
                .ForAllMembers(opt => opt.Ignore());

            // OfferTemplate Mapping
            CreateMap<DomainOfferTemplate, DatabaseOfferTemplate>()
                .ForMember(
                database => database.OfferTemplateId,
                opt => opt.Ignore());

            CreateMap<DatabaseOfferTemplate, DomainOfferTemplate>()
                .ConstructUsing(db => new DomainOfferTemplate.Builder()
                    .SetId(db.OfferTemplateId)
                    .SetCompanyId(db.CompanyId)
                    .SetCreated(db.Created)
                    .SetRemoved(db.Removed)
                    .SetName(db.Name)
                    .SetDescription(db.Description)
                    .SetSkills(db.OfferSkills
                        .Where(os => os.Removed == null)
                        .Select(os => new OfferSkillInfo
                        {
                            Id = os.OfferSkillId,
                            SkillId = os.SkillId,
                            IsRequired = os.IsRequired,
                            Created = os.Created,
                        }))
                    .Build())
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<DomainContractCondition, DatabaseContractCondition>()
                .ConstructUsing(domain => new DatabaseContractCondition
                {
                    CompanyId = domain.CompanyId,
                    SalaryMin = domain.SalaryRange.Min.Value,
                    SalaryMax = domain.SalaryRange.Max.Value,
                    HoursPerTerm = domain.HoursPerTerm.Value,
                    IsNegotiable = domain.IsNegotiable,
                    Created = domain.Created,
                })
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<DatabaseContractCondition, DomainContractCondition>()
                .ConstructUsing(db => new DomainContractCondition.Builder()
                    .SetId(db.ContractConditionId)
                    .SetCompanyId(db.CompanyId)
                    .SetSalaryRange(
                        (db.SalaryMin),
                        (db.SalaryMax))
                    .SetHoursPerTerm(db.HoursPerTerm)
                    .SetIsNegotiable(db.IsNegotiable)
                    .SetCreated(db.Created)
                    .SetRemoved(db.Removed)
                    .SetContractParameters(
                        (ContractAttributeInfo?)(db.ContractAttributes
                            .Where(ca =>
                                ca.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.SalaryTerm &&
                                ca.Removed == null)
                            .OrderBy(ca => ca.Created)
                            .Select(ca => ca.ContractParameterId)
                            .FirstOrDefault()),
                        (ContractAttributeInfo?)(db.ContractAttributes
                            .Where(ca =>
                                ca.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.Currency &&
                                ca.Removed == null)
                            .OrderBy(ca => ca.Created)
                            .Select(ca => ca.ContractParameterId)
                            .FirstOrDefault()),
                        db.ContractAttributes
                            .Where(ca =>
                                ca.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.WorkMode &&
                                ca.Removed == null)
                            .Select(ca => (ContractAttributeInfo)ca.ContractParameterId)
                            .ToList(),
                        db.ContractAttributes
                            .Where(ca =>
                                ca.ContractParameter.ContractParameterTypeId == (int)ContractParameterTypes.EmploymentType &&
                                ca.Removed == null)
                            .Select(ca => (ContractAttributeInfo)ca.ContractParameterId)
                            .ToList()
                            )
                    .Build())
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<DomainOffer, DatabaseOffer>()
                .ConstructUsing(domain => new DatabaseOffer
                {
                    BranchId = domain.BranchId == null
                        ? null : domain.BranchId.Value,
                    PublicationStart = domain.PublicationRange.Start,
                    PublicationEnd = domain.PublicationRange.End,
                    EmploymentLength = domain.EmploymentLength == null
                        ? null
                        : domain.EmploymentLength.Value,
                    WebsiteUrl = domain.WebsiteUrl == null
                        ? null
                        : domain.WebsiteUrl.ToString(),
                })
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<DatabaseOffer, DomainOffer>()
                .ConstructUsing(db =>
                    MapDatabaseOfferToDomainOffer(db))
                .ForAllMembers(opt => opt.Ignore());
        }

        private DomainOffer MapDatabaseOfferToDomainOffer(DatabaseOffer db)
        {
            return new DomainOffer.Builder()
                     .SetId(db.OfferId)
                     .SetBranchId(db.BranchId)
                     .SetPublicationRange(db.PublicationStart, db.PublicationEnd)
                     .SetEmploymentLength(db.EmploymentLength)
                     .SetWebsiteUrl(db.WebsiteUrl)
                     .SetOfferTemplate(
                         db.OfferConnections
                         .Where(oc => oc.Removed == null)
                         .OrderBy(oc => oc.Created)
                         .Select(oc => new TemplateInfo
                         {
                             Id = oc.OfferConnectionId,
                             OfferTemplateId = oc.OfferTemplateId,
                             Removed = oc.Removed,
                             Created = oc.Created,
                         })
                         .First())
                     .SetContractConditions(
                         db.OfferConditions
                         .Where(oc => oc.Removed == null)
                         .Select(oc => new ContractInfo
                         {
                             Id = oc.OfferConditionId,
                             ContractConditionId = oc.ContractConditionId,
                             Removed = oc.Removed,
                             Created = oc.Created,
                         }))
                     .Build();
        }
    }
}
