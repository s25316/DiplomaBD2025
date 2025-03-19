// Ignore Spelling: Enums

using System.ComponentModel;

namespace UseCase.Shared.Enums
{
    public enum ContractParameterTypes
    {
        [Description("Work Mode")]
        WorkMode = 1,

        [Description("Employment Type")]
        EmploymentType = 2,

        [Description("Salary Term")]
        SalaryTerm = 3,

        [Description("Currency")]
        Currency = 4,
    }
}
