namespace UseCase.RelationalDatabase.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public string Name { get; set; } = null!;

    public int SkillTypeId { get; set; }

    public virtual ICollection<OfferSkill> OfferSkills { get; set; } = new List<OfferSkill>();

    public virtual ICollection<PersonSkill> PersonSkills { get; set; } = new List<PersonSkill>();

    public virtual SkillType SkillType { get; set; } = null!;

    public virtual ICollection<Skill> ChildSkills { get; set; } = new List<Skill>();

    public virtual ICollection<Skill> ParentSkills { get; set; } = new List<Skill>();
}
