using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class WorkType
{
    public int WorkTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CompanyOfferWork> CompanyOfferWorks { get; set; } = new List<CompanyOfferWork>();
}
