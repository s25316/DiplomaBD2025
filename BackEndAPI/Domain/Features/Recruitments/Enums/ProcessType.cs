using System.ComponentModel;

namespace Domain.Features.Recruitments.Enums
{
    public enum ProcessType
    {
        [Description("Recruit")]
        Recruit = 1,

        [Description("Watched")]
        Watched = 2,

        [Description("Rejected")]
        Rejected = 11,

        [Description("Passed")]
        Passed = 12,
    }
}
