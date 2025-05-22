namespace UseCase.RelationalDatabase.Models;

public partial class HrProcess
{
    public Guid ProcessId { get; set; }

    public Guid OfferId { get; set; }

    public Guid PersonId { get; set; }

    public int ProcessTypeId { get; set; }

    public string? Message { get; set; }

    public string File { get; set; } = null!;

    public DateTime Created { get; set; }

    public virtual ICollection<Hrchat> Hrchats { get; set; } = new List<Hrchat>();

    public virtual Offer Offer { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ProcessType ProcessType { get; set; } = null!;
}
