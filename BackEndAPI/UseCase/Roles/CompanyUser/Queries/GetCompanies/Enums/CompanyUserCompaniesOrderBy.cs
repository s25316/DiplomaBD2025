// Ignore Spelling: Enums

using System.ComponentModel;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums
{
    public enum CompanyUserCompaniesOrderBy
    {
        [Description("By Name")]
        Name = 1,

        [Description("By Created Date")]
        Created = 2,
    }
}
