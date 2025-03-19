namespace UseCase.RelationalDatabase.Models;

public partial class ContractParameterType
{
    public int ContractParameterTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ContractParameter> ContractParameters { get; set; } = new List<ContractParameter>();
}
