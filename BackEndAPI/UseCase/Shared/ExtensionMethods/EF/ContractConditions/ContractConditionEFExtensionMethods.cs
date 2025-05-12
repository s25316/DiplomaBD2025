using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExtensionMethods
    {
        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="query"></param>
        /// <param name="contractParameterIds"></param>
        /// <param name="salaryParameters"></param>
        /// <returns></returns>
        public static IQueryable<ContractCondition> WhereContractParameters(
           this IQueryable<ContractCondition> query,
           IEnumerable<int> contractParameterIds)
        {
            var expression = ContractConditionEFExpressions.ContractParametersExpression(contractParameterIds);
            return query.Where(expression);
        }

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
