// Ignore Spelling: Enums

using System.ComponentModel;

namespace Domain.Features.Offers.Enums
{
    public enum OfferStatusEnum
    {
        [Description("Offer have no data about publishing")]
        Undefined = 0,
        [Description("Offer Expired")]
        Expired = 1,
        [Description("Offer Active")]
        Active = 2,
        [Description("Offer will active in Future")]
        Future = 3,
    }
}
