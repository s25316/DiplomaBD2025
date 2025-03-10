// Ignore Spelling: Enums

using System.ComponentModel;

namespace UseCase.Shared.Enums
{
    public enum OfferTemplatesOrderBy
    {
        [Description("By Company Name")]
        CompanyName = 1,

        [Description("By Company Created")]
        CompanyCreated = 2,

        [Description("By Offer Template Name")]
        OfferTemplateName = 3,

        [Description("By Offer Template Created")]
        OfferTemplateCreated = 4,

        [Description("By Offer Template Removed")]
        OfferTemplateRemoved = 5,

        [Description("By Offer Skills")]
        Skills = 6,
    }
}
