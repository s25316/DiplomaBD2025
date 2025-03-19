namespace UseCase.RelationalDatabase.Models;

public partial class OfferTemplate
{
    public Guid OfferTemplateId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<OfferConnection> OfferConnections { get; set; } = new List<OfferConnection>();

    public virtual ICollection<OfferSkill> OfferSkills { get; set; } = new List<OfferSkill>();
}
