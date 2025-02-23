using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class EmploymentType
{
    public int EmploymentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OfferEmploymentType> OfferEmploymentTypes { get; set; } = new List<OfferEmploymentType>();
}
