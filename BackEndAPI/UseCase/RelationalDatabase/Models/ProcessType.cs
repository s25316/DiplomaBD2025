namespace UseCase.RelationalDatabase.Models;

public partial class ProcessType
{
    public int ProcessTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HrProcess> HrProcesses { get; set; } = new List<HrProcess>();
}
