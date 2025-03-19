// Ignore Spelling: Enums

using System.ComponentModel;

namespace Domain.Features.Offers.Enums
{
    public enum OfferStatus
    {
        [Description("Offer have no data about publishing")]
        Undefined = 0,

        [Description("Started")]
        Started = 1,

        [Description("Not Started")]
        NotStarted = 2,
    }
}
