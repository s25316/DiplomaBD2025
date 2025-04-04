﻿using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.ContractConditions
{
    public static class ContractConditionEFExtensionMethods
    {
        public static IQueryable<ContractCondition> ContractParametersAndSalaryFilter(
           this IQueryable<ContractCondition> query,
           IEnumerable<int> contractParameterIds,
           SalaryQueryParametersDto salaryParameters)
        {
            var expression = ContractConditionEFExpressions
                .ContractParametersAndSalaryExpression(contractParameterIds, salaryParameters);
            return query.Where(expression);
        }
    }
}
