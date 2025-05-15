// Ignore Spelling: Enums
using System.ComponentModel;

namespace UseCase.Shared.Enums
{
    public enum OfferOrderBy
    {
        [Description("By Publication Start")]
        PublicationStart = 1,

        [Description("By Publication End")]
        PublicationEnd,

        [Description("By Point, if Lon and Lan has filed ")]
        Point,

        [Description("By Offer Skills")]
        Skills,

        [Description("By Contract Parameters Count")]
        ContractParameters,

        [Description("By Min Salary")]
        SalaryMin,

        [Description("By Max Salary")]
        SalaryMax,

        [Description("By AVG Salary")]
        SalaryAvg,

        [Description("By Per Hour Min Salary")]
        SalaryPerHourMin,

        [Description("By Per Hour Max Salary")]
        SalaryPerHourMax,

        [Description("By Per Hour AVG Salary")]
        SalaryPerHourAvg,
    }
}
