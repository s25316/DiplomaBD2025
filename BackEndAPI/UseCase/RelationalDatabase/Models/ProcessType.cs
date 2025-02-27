namespace UseCase.RelationalDatabase.Models;

public partial class ProcessType
{
    public int ProcessTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Hrchat> Hrchats { get; set; } = new List<Hrchat>();
}
