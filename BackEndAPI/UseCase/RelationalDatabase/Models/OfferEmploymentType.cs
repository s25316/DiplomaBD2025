namespace UseCase.RelationalDatabase.Models;

public partial class OfferEmploymentType
{
    public Guid OfferEmploymentTypeId { get; set; }

    public Guid OfferId { get; set; }

    public int EmploymentTypeId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual EmploymentType EmploymentType { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;
}
