using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class CompanyOffer
{
    public Guid CompanyOfferId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid OfferId { get; set; }

    public Guid? BranchId { get; set; }

    public DateTime PublishStart { get; set; }

    public DateTime? PublishEnd { get; set; }

    public DateOnly? WorkStart { get; set; }

    public DateOnly? WorkEnd { get; set; }

    public decimal MinSalary { get; set; }

    public decimal MaxSalary { get; set; }

    public bool IsNegotiated { get; set; }

    public string? Url { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CompanyOfferContract> CompanyOfferContracts { get; set; } = new List<CompanyOfferContract>();

    public virtual ICollection<CompanyOfferWork> CompanyOfferWorks { get; set; } = new List<CompanyOfferWork>();

    public virtual ICollection<Hrprocess> Hrprocesses { get; set; } = new List<Hrprocess>();

    public virtual Offer Offer { get; set; } = null!;
}
