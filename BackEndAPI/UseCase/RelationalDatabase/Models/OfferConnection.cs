namespace UseCase.RelationalDatabase.Models;

public partial class OfferConnection
{
    public Guid OfferConnectionId { get; set; }

    public Guid OfferTemplateId { get; set; }

    public Guid OfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual OfferTemplate OfferTemplate { get; set; } = null!;
}
