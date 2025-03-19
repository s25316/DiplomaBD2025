namespace UseCase.RelationalDatabase.Models;

public partial class ContractParameter
{
    public int ContractParameterId { get; set; }

    public string Name { get; set; } = null!;

    public int ContractParameterTypeId { get; set; }

    public virtual ICollection<ContractAttribute> ContractAttributes { get; set; } = new List<ContractAttribute>();

    public virtual ContractParameterType ContractParameterType { get; set; } = null!;
}
