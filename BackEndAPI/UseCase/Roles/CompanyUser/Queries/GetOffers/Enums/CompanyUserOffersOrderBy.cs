using System.ComponentModel;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers.Enums
{
    public enum CompanyUserOffersOrderBy
    {
        Undefined = 1,
        PublicationStart = 2,
        PublicationEnd = 2,
        [Description("By Offer Skills")]
        Skills = 6,
        [Description("By Point, if Lon and Lan has filed ")]
        Point = 6,
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
    }
}
