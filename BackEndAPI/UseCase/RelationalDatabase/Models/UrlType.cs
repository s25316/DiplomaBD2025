namespace UseCase.RelationalDatabase.Models;

public partial class UrlType
{
    public int UrlTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
