namespace UseCase.RelationalDatabase.Models;

public partial class ContractAttribute
{
    public Guid ContractAttributeId { get; set; }

    public int ContractParameterId { get; set; }

    public Guid ContractConditionId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual ContractCondition ContractCondition { get; set; } = null!;

    public virtual ContractParameter ContractParameter { get; set; } = null!;
}
