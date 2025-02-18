using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Offer
{
    public Guid OfferId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual ICollection<CompanyOffer> CompanyOffers { get; set; } = new List<CompanyOffer>();

    public virtual ICollection<OfferSkill> OfferSkills { get; set; } = new List<OfferSkill>();
}
