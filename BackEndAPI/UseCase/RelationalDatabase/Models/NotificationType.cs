namespace UseCase.RelationalDatabase.Models;

public partial class NotificationType
{
    public int NotificationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
