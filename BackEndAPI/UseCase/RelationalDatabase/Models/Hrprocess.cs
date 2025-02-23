using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Hrprocess
{
    public Guid ProcessId { get; set; }

    public Guid PersonId { get; set; }

    public Guid OfferId { get; set; }

    public virtual ICollection<Hrchat> Hrchats { get; set; } = new List<Hrchat>();

    public virtual Offer Offer { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
