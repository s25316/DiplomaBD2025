namespace UseCase.RelationalDatabase.Models;

public partial class ContractCondition
{
    public Guid ContractConditionId { get; set; }

    public Guid CompanyId { get; set; }

    public decimal SalaryMin { get; set; }

    public decimal SalaryMax { get; set; }

    public int HoursPerTerm { get; set; }

    public bool IsNegotiable { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<ContractAttribute> ContractAttributes { get; set; } = new List<ContractAttribute>();

    public virtual ICollection<OfferCondition> OfferConditions { get; set; } = new List<OfferCondition>();
}
