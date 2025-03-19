namespace UseCase.RelationalDatabase.Models;

public partial class OfferCondition
{
    public Guid OfferConditionId { get; set; }

    public Guid ContractConditionId { get; set; }

    public Guid OfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual ContractCondition ContractCondition { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;
}
