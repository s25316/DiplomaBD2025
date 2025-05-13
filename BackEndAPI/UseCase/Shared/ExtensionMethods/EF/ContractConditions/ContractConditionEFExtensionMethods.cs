using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExtensionMethods
    {
        public static IQueryable<ContractCondition> WhereSalary(
           this IQueryable<ContractCondition> query,
           SalaryQueryParametersDto salaryParameters)
        {
            if (!salaryParameters.HasValue)
            {
                return query;
            }
            var expression = ContractConditionEFExpressions.SalaryExpression(salaryParameters);
            return query.Where(expression);
        }
    }
}
