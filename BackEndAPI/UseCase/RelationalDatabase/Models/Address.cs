namespace UseCase.RelationalDatabase.Models;
using NetTopologySuite.Geometries;

public partial class Address
{
    public Guid AddressId { get; set; }

    public int CityId { get; set; }

    public int? StreetId { get; set; }

    public string HouseNumber { get; set; } = null!;

    public string? ApartmentNumber { get; set; }

    public string PostCode { get; set; } = null!;

    public float Lon { get; set; }

    public float Lat { get; set; }

    public Point Point { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual Street? Street { get; set; }
}
