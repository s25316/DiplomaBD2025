using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExtensionMethods
    {
        public static IQueryable<ContractCondition> ContractParametersAndSalaryFilter(
           this IQueryable<ContractCondition> query,
           IEnumerable<int> parameterIds,
           SalaryQueryParametersDto salary)
        {
            var expression = ContractConditionEFExpressions
                .ContractParametersAndSalaryExpression(parameterIds, salary);
            return query.Where(expression);
        }
    }
}
