using AutoMapper;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DatabaseContractCondition = UseCase.RelationalDatabase.Models.ContractCondition;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainBranch = Domain.Features.Branches.Entities.Branch;
using DomainCompany = Domain.Features.Companies.Entities.Company;
using DomainContractCondition = Domain.Features.ContractConditions.Entities.ContractCondition;
using DomainOffer = Domain.Features.Offers.Entities.Offer;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.AutoMapperProfile
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<DomainCompany, DatabaseCompany>()
                .ForMember(
                database => database.CompanyId,
                opt => opt.Ignore());

            CreateMap<DomainBranch, DatabaseBranch>()
                .ForMember(
                database => database.BranchId,
                opt => opt.Ignore());

            CreateMap<DomainOfferTemplate, DatabaseOfferTemplate>()
                .ForMember(
                database => database.OfferTemplateId,
                opt => opt.Ignore());

            CreateMap<DomainContractCondition, DatabaseContractCondition>()
                .ConstructUsing(domain => new DatabaseContractCondition
                {
                    CompanyId = domain.CompanyId,
                    SalaryMin = domain.SalaryRange.Min.Value,
                    SalaryMax = domain.SalaryRange.Max.Value,
                    HoursPerTerm = domain.HoursPerTerm.Value,
                    IsNegotiable = domain.IsNegotiable,
                    Created = domain.Created,
                });

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
                        : domain.WebsiteUrl.Value,
                });
        }
    }
}
