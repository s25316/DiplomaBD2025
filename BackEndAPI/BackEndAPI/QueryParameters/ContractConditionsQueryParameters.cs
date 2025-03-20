using UseCase.Shared.ValidationAttributes.ContractConditionAttributes;

namespace BackEndAPI.QueryParameters
{
    public sealed class ContractConditionsQueryParameters
    {
        public bool? IsNegotiable { get; init; }

        public bool? IsPaid { get; init; }

        [Money]
        public decimal? SalaryPerHourMin { get; init; }

        [Money]
        public decimal? SalaryPerHourMax { get; init; }

        [Money]
        public decimal? SalaryMin { get; init; }

        [Money]
        public decimal? SalaryMax { get; init; }

        [Hour]
        public int? HoursMin { get; init; }

        [Hour]
        public int? HoursMax { get; init; }
    }
}
