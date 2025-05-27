using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.ExtensionMethods.EF.Offers;
using UseCase.Shared.ExtensionMethods.EF.OfferTemplates;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Recruitments
{
    public static class HrProcessEFExtensionMethods
    {
        public static IQueryable<HrProcess> WhereRecruitmentId(
            this IQueryable<HrProcess> query,
            Guid recruitmentId)
        {
            return query.Where(r => r.ProcessId == recruitmentId);
        }

        public static IQueryable<HrProcess> WhereSalary(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            SalaryQueryParametersDto parameters)
        {
            if (parameters.HasValue)
            {
                return query.Where(recruitment =>
                    context.ContractConditions
                        .Include(cc => cc.OfferConditions)
                        .WhereSalary(parameters)
                        .Any(cc =>
                            cc.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId
                        )));
            }
            return query;
        }

        public static IQueryable<HrProcess> WhereContractParameters(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            IEnumerable<int> contractParameterIds)
        {
            if (contractParameterIds.Any())
            {
                return query.Where(recruitment =>
                    contractParameterIds.Any(contractParameterId =>
                         context.ContractAttributes
                         .Include(ca => ca.ContractCondition)
                         .ThenInclude(cc => cc.OfferConditions)
                         .Any(ca =>
                            ca.Removed == null &&
                            ca.ContractParameterId == contractParameterId &&
                            ca.ContractCondition.OfferConditions.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId
                         ))
                    ));
            }
            return query;
        }

        public static IQueryable<HrProcess> WhereSkills(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            IEnumerable<int> skillIds)
        {
            if (skillIds.Any())
            {
                return query.Where(recruitment =>
                    skillIds.Any(skillId =>
                        context.OfferSkills
                        .Include(os => os.OfferTemplate)
                        .ThenInclude(ot => ot.OfferConnections)
                        .Any(os =>
                            os.Removed == null &&
                            os.SkillId == skillId &&
                            os.OfferTemplate.OfferConnections.Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId
                            )
                        )
                    )
                );
            }
            return query;
        }

        public static IQueryable<HrProcess> WhereText(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            string? searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                return query.Where(recruitment =>
                    context.OfferTemplates
                    .Include(ot => ot.Company)
                    .Include(ot => ot.OfferConnections)
                    .WhereText(searchText)
                    .Any(ot => ot.OfferConnections.Any(oc =>
                        oc.Removed == null &&
                        oc.OfferId == recruitment.OfferId
                    )) ||
                    context.Branches
                    .Include(branch => branch.Company)
                    .WhereText(searchText)
                    .Any(branch =>
                        recruitment.Offer.BranchId == branch.BranchId)
                );
            }
            return query;
        }

        public static IQueryable<HrProcess> WhereOfferParameters(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            OfferQueryParametersDto offerParameters)
        {
            return query
                .Where(recruitment => context.Offers
                    .WhereOfferParameters(offerParameters)
                    .Any(offer => offer.OfferId == recruitment.OfferId)
                );
        }

        public static IQueryable<HrProcess> WhereCompanyIdentificationData(
            this IQueryable<HrProcess> query,
            DiplomaBdContext context,
            Guid? companyId,
            CompanyQueryParametersDto companyQueryParameters)
        {
            return query.Where(recruitment => context.Companies
                .Include(x => x.OfferTemplates)
                .ThenInclude(x => x.OfferConnections)
                .WhereIdentificationData(companyId, companyQueryParameters)
                .Any(company => company.OfferTemplates
                    .Any(ot =>
                        ot.OfferConnections.Any(oc =>
                            oc.OfferId == recruitment.OfferId &&
                            oc.Removed == null
                ))));
        }

        public static IQueryable<HrProcess> WhereProcessType(
            this IQueryable<HrProcess> query,
            Domain.Features.Recruitments.Enums.ProcessType? processType)
        {
            Expression<Func<HrProcess, bool>> expression = recruitment =>
                !processType.HasValue ||
                recruitment.ProcessTypeId == (int)processType;

            return query.Where(expression);
        }
    }
}
