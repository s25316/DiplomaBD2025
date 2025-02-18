using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class OfferSkill
{
    public Guid OfferId { get; set; }

    public int SkillId { get; set; }

    public bool IsRequired { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
