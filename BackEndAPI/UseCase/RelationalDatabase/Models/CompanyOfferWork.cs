using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class CompanyOfferWork
{
    public int WorkId { get; set; }

    public int WorkTypeId { get; set; }

    public Guid CompanyOfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual CompanyOffer CompanyOffer { get; set; } = null!;

    public virtual WorkType WorkType { get; set; } = null!;
}
