namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Request
{
    public class OfferCreateCommand
    {
        public Guid? BranchId { get; init; }
        public DateTime PublicationStart { get; init; }
        public DateTime? PublicationEnd { get; init; }
        public DateTime? WorkBeginDate { get; init; }
        public DateTime? WorkEndDate { get; init; }
        public decimal SalaryRangeMin { get; init; }
        public decimal SalaryRangeMax { get; init; }
        public int? SalaryTermId { get; init; }
        public int? CurrencyId { get; init; }
        public bool IsNegotiated { get; init; }
        public string? WebsiteUrl { get; init; }
        public IEnumerable<int> EmploymentTypeIds { get; init; } = [];
        public IEnumerable<int> WorkModeIds { get; init; } = [];
    }
}
