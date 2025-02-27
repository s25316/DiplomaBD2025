namespace UseCase.RelationalDatabase.Models;

public partial class WorkMode
{
    public int WorkModeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OfferWorkMode> OfferWorkModes { get; set; } = new List<OfferWorkMode>();
}
