using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Branch
{
    public Guid BranchId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid AddressId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
