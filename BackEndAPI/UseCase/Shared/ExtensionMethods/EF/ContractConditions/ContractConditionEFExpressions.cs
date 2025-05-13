using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExpressions
    {
        public static Expression<Func<ContractCondition, bool>> SalaryExpression(
          SalaryQueryParametersDto salaryParameters)
        {
            return cc =>
                    (
                        !salaryParameters.IsNegotiable.HasValue ||
                        (
                            salaryParameters.IsNegotiable == true
                                ? cc.IsNegotiable
                                : !cc.IsNegotiable
                        )
                    ) &&
                    (
                        !salaryParameters.IsPaid.HasValue ||
                        (
                            salaryParameters.IsPaid == true
                                ? cc.SalaryMin > 0
                                : cc.SalaryMax <= 0
                        )
                    ) &&
                    (
                        !salaryParameters.SalaryPerHourMin.HasValue ||
                        ((cc.SalaryMin / cc.HoursPerTerm) >= salaryParameters.SalaryPerHourMin)
                    ) &&
                    (
                            !salaryParameters.SalaryPerHourMax.HasValue ||
                        ((cc.SalaryMax / cc.HoursPerTerm) <= salaryParameters.SalaryPerHourMax)
                    ) &&
                    (
                        !salaryParameters.SalaryMin.HasValue ||
                        (cc.SalaryMin >= salaryParameters.SalaryMin)
                    ) &&
                    (
                        !salaryParameters.SalaryMax.HasValue ||
                        (cc.SalaryMax <= salaryParameters.SalaryMax)
                    ) &&
                    (
                        !salaryParameters.HoursMin.HasValue ||
                        (cc.HoursPerTerm >= salaryParameters.HoursMin)
                    ) &&
                    (
                        !salaryParameters.HoursMax.HasValue ||
                        (cc.HoursPerTerm <= salaryParameters.HoursMax)
                    );
        }
    }
}
