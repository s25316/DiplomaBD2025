// Ignore Spelling: Dto

using UseCase.Shared.ValidationAttributes.ContractConditionAttributes;

namespace UseCase.Shared.DTOs.QueryParameters
{
    public sealed class SalaryQueryParametersDto
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


        // Methods
        public bool ContainsAny()
        {
            return
                IsNegotiable.HasValue ||
                IsPaid.HasValue ||
                SalaryPerHourMin.HasValue ||
                SalaryPerHourMax.HasValue ||
                SalaryMin.HasValue ||
                SalaryMax.HasValue ||
                HoursMin.HasValue ||
                HoursMax.HasValue;
        }
    }
}
