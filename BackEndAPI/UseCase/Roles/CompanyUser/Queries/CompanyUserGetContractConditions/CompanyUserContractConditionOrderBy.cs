// Ignore Spelling: Enums

using System.ComponentModel;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions
{
    public enum CompanyUserContractConditionOrderBy
    {
        [Description("By Company Name")]
        CompanyName = 1,

        [Description("By Company Created")]
        CompanyCreated = 2,

        [Description("By Contract Condition Created")]
        ContractConditionCreated = 3,

        [Description("By Contract Condition Removed")]
        ContractConditionRemoved = 4,

        [Description("By Min Salary")]
        SalaryMin = 5,

        [Description("By Max Salary")]
        SalaryMax = 6,

        [Description("By AVG Salary")]
        SalaryAvg = 7,

        [Description("By Per Hour Min Salary")]
        SalaryPerHourMin = 8,

        [Description("By Per Hour Max Salary")]
        SalaryPerHourMax = 9,

        [Description("By Per Hour AVG Salary")]
        SalaryPerHourAvg = 10,

        [Description("By Contract Parameters Count")]
        ContractParameters = 11,
    }
}
