using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class CompanyPerson
{
    public Guid CompanyPersonId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid PersonId { get; set; }

    public int RoleId { get; set; }

    public DateTime Grant { get; set; }

    public DateTime? Deny { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
