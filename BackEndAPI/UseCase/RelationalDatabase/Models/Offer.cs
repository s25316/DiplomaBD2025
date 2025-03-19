namespace UseCase.RelationalDatabase.Models;

public partial class Offer
{
    public Guid OfferId { get; set; }

    public Guid? BranchId { get; set; }

    public DateTime PublicationStart { get; set; }

    public DateTime? PublicationEnd { get; set; }

    public float? EmploymentLength { get; set; }

    public string? WebsiteUrl { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<OfferCondition> OfferConditions { get; set; } = new List<OfferCondition>();

    public virtual ICollection<OfferConnection> OfferConnections { get; set; } = new List<OfferConnection>();
}
