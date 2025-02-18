using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Hrprocess
{
    public Guid ProcessId { get; set; }

    public Guid CompanyOfferId { get; set; }

    public Guid PersonId { get; set; }

    public virtual CompanyOffer CompanyOffer { get; set; } = null!;

    public virtual ICollection<Hrchat> Hrchats { get; set; } = new List<Hrchat>();

    public virtual Person Person { get; set; } = null!;
}
