// Ignore Spelling: Enums

using System.ComponentModel;

namespace Domain.Features.Offers.ValueObjects.Enums
{
    public enum OfferStatus
    {
        [Description("Offer have no data about publishing")]
        Undefined = 0,

        [Description("Expired")]
        Expired = 1,

        [Description("Active")]
        Active = 2,

        [Description("Scheduled")]
        Scheduled = 3,
    }
}
