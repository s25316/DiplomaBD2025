using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class SkillType
{
    public int SkillTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
