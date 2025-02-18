using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class ContractType
{
    public int ContractTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CompanyOfferContract> CompanyOfferContracts { get; set; } = new List<CompanyOfferContract>();
}
