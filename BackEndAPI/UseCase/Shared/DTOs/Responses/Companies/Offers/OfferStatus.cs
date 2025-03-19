using System.ComponentModel;

namespace UseCase.Shared.DTOs.Responses.Companies.Offers
{
    public enum OfferStatus
    {
        [Description("Expired")]
        Expired = 1,

        [Description("Active")]
        Active = 2,

        [Description("Pending")]
        Pending = 3,
    }
}
