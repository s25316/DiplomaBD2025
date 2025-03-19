using Domain.Features.ContractConditions.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using System.Text;

namespace Domain.Features.ContractConditions.Entities
{
    public partial class ContractCondition : TemplateEntity<ContractConditionId>
    {
        public class Builder : TemplateBuilder<ContractCondition>
        {
            public Builder SetId(Guid contractConditionId)
            {
                SetProperty(cc => cc.Id = contractConditionId);
                return this;
            }

            public Builder SetCompanyId(Guid companyId)
            {
                SetProperty(cc => cc.CompanyId = companyId);
                return this;
            }

            public Builder SetSalaryRange(
                decimal minSalary,
                decimal maxSalary)
            {
                SetProperty(cc => cc.SetSalaryRange(minSalary, maxSalary));
                return this;
            }

            public Builder SetHoursPerTerm(int hoursPerTerm)
            {
                SetProperty(cc => cc.HoursPerTerm = hoursPerTerm);
                return this;
            }

            public Builder SetIsNegotiable(bool isNegotiable)
            {
                SetProperty(cc => cc.IsNegotiable = isNegotiable);
                return this;
            }

            public Builder SetCreated(DateTime dateTime)
            {
                SetProperty(cc => cc.Created = dateTime);
                return this;
            }

            public Builder SetRemoved(DateTime? dateTime)
            {
                SetProperty(cc => cc.Removed = dateTime);
                return this;
            }

            public Builder SetContractParameters(
                int? salaryTermId,
                int? currencyId,
                IEnumerable<int> workModeIds,
                IEnumerable<int> employmentTypeIds)
            {
                SetProperty(cc => cc.SetContractParameters(
                    salaryTermId,
                    currencyId,
                    workModeIds,
                    employmentTypeIds));
                return this;
            }

            // Protected Methods
            protected override Func<ContractCondition, string> CheckIsObjectComplete()
            {
                return cc =>
                {
                    var builder = new StringBuilder();
                    if (cc.CompanyId == null)
                    {
                        builder.AppendLine(nameof(cc.CompanyId));
                    }
                    return builder.ToString();
                };
            }

            protected override Action<ContractCondition> SetDefaultValues()
            {
                return cc =>
                {
                    if (cc.SalaryRange == null)
                    {
                        cc.SalaryRange = new SalaryRange(0, 0);
                    }
                    if (cc.HoursPerTerm == null)
                    {
                        cc.HoursPerTerm = 1;
                    }
                    if (cc.Created == DateTime.MinValue)
                    {
                        cc.Created = CustomTimeProvider.GetDateTimeNow();
                    }
                };
            }
        }
    }
}
