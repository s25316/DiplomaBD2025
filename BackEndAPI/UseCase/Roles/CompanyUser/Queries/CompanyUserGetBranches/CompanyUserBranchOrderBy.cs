// Ignore Spelling: Enums, Lan

using System.ComponentModel;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches
{
    public enum CompanyUserBranchOrderBy
    {
        [Description("By Company Name")]
        CompanyName = 1,

        [Description("By Company Created")]
        CompanyCreated = 2,

        [Description("By Branch Name")]
        BranchName = 3,

        [Description("By Branch Created")]
        BranchCreated = 4,

        [Description("By Branch Removed")]
        BranchRemoved = 5,

        [Description("By Point, if Lon and Lan has filed ")]
        Point = 6,
    }
}
