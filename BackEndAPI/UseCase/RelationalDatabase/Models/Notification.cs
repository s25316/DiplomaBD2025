namespace UseCase.RelationalDatabase.Models;

public partial class Notification
{
    public Guid NotificationId { get; set; }

    public int NotificationTypeId { get; set; }

    public Guid? PersonId { get; set; }

    public bool IsAdminSend { get; set; }

    public string? Email { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public DateTime? Read { get; set; }

    public DateTime? Completed { get; set; }

    public Guid? ObjectId { get; set; }

    public virtual ICollection<Nchat> Nchats { get; set; } = new List<Nchat>();

    public virtual NotificationType NotificationType { get; set; } = null!;

    public virtual Person? Person { get; set; }
}
