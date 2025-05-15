// Ignore Spelling: Enums
using System.ComponentModel;

namespace UseCase.Shared.Enums
{
    public enum CompanyOrderBy
    {
        [Description("By Name")]
        Name = 1,

        [Description("By Created Date")]
        Created = 2,
    }
}
