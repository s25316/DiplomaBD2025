﻿namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserCreateContractConditions.Request
{
    public class CompanyUserCreateContractConditionsCommand
    {
        public decimal SalaryMin { get; init; }

        public decimal SalaryMax { get; init; }

        public int HoursPerTerm { get; init; }

        public bool IsNegotiable { get; init; }

        // ContractParameters
        public int? SalaryTermId { get; init; }

        public int? CurrencyId { get; init; }

        public IEnumerable<int> WorkModeIds { get; init; } = [];

        public IEnumerable<int> EmploymentTypeIds { get; init; } = [];
    }
}
