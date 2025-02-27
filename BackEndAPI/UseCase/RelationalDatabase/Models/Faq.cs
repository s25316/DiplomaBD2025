namespace UseCase.RelationalDatabase.Models;

public partial class Faq
{
    public Guid FaqId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }
}
