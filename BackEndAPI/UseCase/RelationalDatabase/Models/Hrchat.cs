namespace UseCase.RelationalDatabase.Models;

public partial class Hrchat
{
    public Guid MessageId { get; set; }

    public Guid ProcessId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public DateTime? Read { get; set; }

    public bool IsPersonSend { get; set; }

    public string? Message { get; set; }

    public string? MongoUrl { get; set; }

    public int ProcessTypeId { get; set; }

    public virtual HrProcess Process { get; set; } = null!;

    public virtual ProcessType ProcessType { get; set; } = null!;
}
