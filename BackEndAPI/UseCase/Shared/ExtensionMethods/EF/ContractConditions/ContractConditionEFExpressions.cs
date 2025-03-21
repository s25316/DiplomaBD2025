using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExpressions
    {
        public static Expression<Func<ContractCondition, bool>> ContractParametersAndSalaryExpression(
           IEnumerable<int> parameterIds,
           SalaryQueryParametersDto salary)
        {
            return cc =>
                (
                    !parameterIds.Any() ||
                    parameterIds.Any(parametrId =>
                        cc.ContractAttributes.Any(atribute =>
                            atribute.ContractParameterId == parametrId
                    ))
                ) &&
                (
                    !salary.IsNegotiable.HasValue ||
                    (
                        salary.IsNegotiable == true
                            ? cc.IsNegotiable
                            : !cc.IsNegotiable
                    )
                ) &&
                (
                    !salary.IsPaid.HasValue ||
                    (
                        salary.IsPaid == true
                            ? cc.SalaryMin > 0
                            : cc.SalaryMax <= 0
                    )
                ) &&
                (
                    (
                        !salary.SalaryPerHourMin.HasValue ||
                        ((cc.SalaryMin / cc.HoursPerTerm) >= salary.SalaryPerHourMin)
                    ) &&
                    (
                        !salary.SalaryPerHourMax.HasValue ||
                        ((cc.SalaryMax / cc.HoursPerTerm) <= salary.SalaryPerHourMax)
                    ) &&
                    (
                        !salary.SalaryMin.HasValue ||
                        (cc.SalaryMin >= salary.SalaryMin)
                    ) &&
                    (
                        !salary.SalaryMax.HasValue ||
                        (cc.SalaryMax <= salary.SalaryMax)
                    ) &&
                    (
                        !salary.HoursMin.HasValue ||
                        (cc.HoursPerTerm >= salary.HoursMin)
                    ) &&
                    (
                        !salary.HoursMax.HasValue ||
                        (cc.HoursPerTerm <= salary.HoursMax)
                    )
                );
        }
    }
}
