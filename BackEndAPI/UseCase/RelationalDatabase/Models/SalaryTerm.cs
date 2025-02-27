namespace UseCase.RelationalDatabase.Models;

public partial class SalaryTerm
{
    public int SalaryTermId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
