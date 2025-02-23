using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class OfferWorkMode
{
    public Guid OfferWorkModeId { get; set; }

    public Guid OfferId { get; set; }

    public int WorkModeId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual WorkMode WorkMode { get; set; } = null!;
}
