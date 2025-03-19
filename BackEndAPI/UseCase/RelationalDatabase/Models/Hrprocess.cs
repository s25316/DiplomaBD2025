namespace UseCase.RelationalDatabase.Models;

public partial class HrProcess
{
    public Guid ProcessId { get; set; }

    public virtual ICollection<Hrchat> Hrchats { get; set; } = new List<Hrchat>();
}
