using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class OfferSkill
{
    public Guid OfferSkillId { get; set; }

    public Guid OfferTemplateId { get; set; }

    public int SkillId { get; set; }

    public bool IsRequired { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual OfferTemplate OfferTemplate { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
