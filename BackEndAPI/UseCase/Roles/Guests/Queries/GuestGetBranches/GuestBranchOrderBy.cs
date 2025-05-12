using System.ComponentModel;

namespace UseCase.Roles.Guests.Queries.GuestGetBranches
{
    public enum GuestBranchOrderBy
    {
        [Description("By Company Name")]
        CompanyName = 1,

        [Description("By Company Created")]
        CompanyCreated = 2,

        [Description("By Branch Name")]
        BranchName = 3,

        [Description("By Branch Created")]
        BranchCreated = 4,

        [Description("By Point, if Lon and Lan has filed ")]
        Point = 6,
    }
}
