using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class CompanyOfferContract
{
    public int ContractId { get; set; }

    public int ContractTypeId { get; set; }

    public Guid CompanyOfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual CompanyOffer CompanyOffer { get; set; } = null!;

    public virtual ContractType ContractType { get; set; } = null!;
}
