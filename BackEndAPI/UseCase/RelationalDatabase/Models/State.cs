namespace UseCase.RelationalDatabase.Models;

public partial class State
{
    public int StateId { get; set; }

    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
