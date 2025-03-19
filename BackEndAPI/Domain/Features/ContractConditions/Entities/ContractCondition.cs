using Domain.Features.Companies.ValueObjects;
using Domain.Features.ContractConditions.Exceptions;
using Domain.Features.ContractConditions.ValueObjects;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.Entities
{
    public partial class ContractCondition : TemplateEntity<ContractConditionId>
    {
        // Properties
        public CompanyId CompanyId { get; private set; } = null!;
        public SalaryRange SalaryRange { get; private set; } = null!;
        public HoursPerTerm HoursPerTerm { get; private set; } = null!;
        public bool IsNegotiable { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; } = null;
        public int? SalaryTermId { get; private set; }
        public int? CurrencyId { get; private set; }
        public IEnumerable<int> WorkModeIds { get; private set; } = [];
        public IEnumerable<int> EmploymentTypeIds { get; private set; } = [];


        // Methods
        private void SetSalaryRange(
                decimal minSalary,
                decimal maxSalary)
        {
            SalaryRange = new SalaryRange(minSalary, maxSalary);
        }

        private void SetContractParameters(
            int? salaryTermId,
            int? currencyId,
            IEnumerable<int> workModeIds,
            IEnumerable<int> employmentTypeIds)
        {
            if (SalaryRange != null && SalaryRange.Min > 0)
            {
                if (!salaryTermId.HasValue)
                {
                    throw new ContractConditionException(
                        Messages.Entity_ContractCondition_SalaryTermId_Empty);

                }
                if (!currencyId.HasValue)
                {
                    throw new ContractConditionException(
                        Messages.Entity_ContractCondition_CurrencyId_Empty);
                }
                SalaryTermId = salaryTermId;
                CurrencyId = currencyId;
            }

            WorkModeIds = workModeIds;
            EmploymentTypeIds = employmentTypeIds;
        }
    }
}
