namespace UseCase.RelationalDatabase.Models;

public partial class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public int StateId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual State State { get; set; } = null!;
}
