using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class PersonSkill
{
    public Guid PersonSkillId { get; set; }

    public Guid PersonId { get; set; }

    public int SkillId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
