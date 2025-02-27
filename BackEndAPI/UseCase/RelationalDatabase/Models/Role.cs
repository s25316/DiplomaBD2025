namespace UseCase.RelationalDatabase.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<CompanyPerson> CompanyPeople { get; set; } = new List<CompanyPerson>();
}
