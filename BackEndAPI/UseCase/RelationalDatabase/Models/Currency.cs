using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
