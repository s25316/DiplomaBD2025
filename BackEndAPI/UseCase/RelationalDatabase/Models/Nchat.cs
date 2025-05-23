﻿namespace UseCase.RelationalDatabase.Models;

public partial class Nchat
{
    public Guid MessageId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public DateTime? Read { get; set; }

    public bool IsAdminSend { get; set; }

    public string? Message { get; set; }

    public Guid NotificationId { get; set; }

    public virtual Notification Notification { get; set; } = null!;
}
