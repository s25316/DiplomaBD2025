using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.GetOffers.Response
{
    public class OfferDto
    {
        public Guid OfferId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime PublicationStart { get; set; }
        public DateTime? PublicationEnd { get; set; }
        public DateTime? WorkBeginDate { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public decimal SalaryRangeMin { get; set; }
        public decimal SalaryRangeMax { get; set; }
        public bool IsNegotiated { get; set; }
        public string? WebsiteUrl { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public BranchDto? Branch { get; set; }
        public CurrencyDto? Currency { get; set; }
        public virtual SalaryTermDto? SalaryTerm { get; set; }
        public IEnumerable<OfferSkillResponseDto> Skills { get; set; } = [];
        public IEnumerable<EmploymentTypeDto> EmploymentTypes { get; set; } = [];
        public virtual IEnumerable<WorkModeDto> WorkModes { get; set; } = [];

    }
}
