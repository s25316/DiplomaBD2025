// Ignore Spelling: Dto

using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Shared.DTOs.Responses.Companies
{
    public class OfferDto
    {
        public Guid OfferId { get; init; }
        public Guid OfferTemplateId { get; init; }
        public Guid? BranchId { get; init; }
        public DateTime PublicationStart { get; init; }
        public DateTime? PublicationEnd { get; init; }
        public DateOnly? WorkBeginDate { get; init; }
        public DateOnly? WorkEndDate { get; init; }
        public decimal SalaryRangeMin { get; init; }
        public decimal SalaryRangeMax { get; init; }
        public int? SalaryTermId { get; init; }
        public int? CurrencyId { get; init; }
        public bool IsNegotiated { get; init; }
        public string? WebsiteUrl { get; init; }
        public CurrencyDto? Currency { get; init; }
        public SalaryTermDto? SalaryTerm { get; init; }
        public IEnumerable<WorkModeDto> WorkModes { get; init; } = [];
        public IEnumerable<EmploymentType> EmploymentTypes { get; init; } = [];

        // Branch
    }
}
