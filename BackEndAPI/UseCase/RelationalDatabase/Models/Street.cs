using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Street
{
    public int StreetId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
