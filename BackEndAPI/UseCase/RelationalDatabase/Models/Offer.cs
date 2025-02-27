namespace UseCase.RelationalDatabase.Models;

public partial class Offer
{
    public Guid OfferId { get; set; }

    public Guid OfferTemplateId { get; set; }

    public Guid? BranchId { get; set; }

    public DateTime PublicationStart { get; set; }

    public DateTime? PublicationEnd { get; set; }

    public DateOnly? WorkBeginDate { get; set; }

    public DateOnly? WorkEndDate { get; set; }

    public decimal SalaryRangeMin { get; set; }

    public decimal SalaryRangeMax { get; set; }

    public int? SalaryTermId { get; set; }

    public int? CurrencyId { get; set; }

    public bool IsNegotiated { get; set; }

    public string? WebsiteUrl { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<Hrprocess> Hrprocesses { get; set; } = new List<Hrprocess>();

    public virtual ICollection<OfferEmploymentType> OfferEmploymentTypes { get; set; } = new List<OfferEmploymentType>();

    public virtual OfferTemplate OfferTemplate { get; set; } = null!;

    public virtual ICollection<OfferWorkMode> OfferWorkModes { get; set; } = new List<OfferWorkMode>();

    public virtual SalaryTerm? SalaryTerm { get; set; }
}
