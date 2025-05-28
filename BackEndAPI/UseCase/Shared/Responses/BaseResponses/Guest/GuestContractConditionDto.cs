using UseCase.Shared.Dictionaries.GetContractParameters.Response;

namespace UseCase.Shared.Responses.BaseResponses.Guest
{
    public class GuestContractConditionDto
    {
        public Guid ContractConditionId { get; init; }

        public Guid CompanyId { get; init; }

        public int HoursPerTerm { get; init; }

        public decimal SalaryMin { get; init; }

        public decimal SalaryMax { get; init; }

        public decimal SalaryAvg { get; init; }

        public decimal SalaryPerHourAvg { get; init; }

        public decimal SalaryPerHourMin { get; init; }

        public decimal SalaryPerHourMax { get; init; }

        public bool IsPaid { get; init; }

        public bool IsNegotiable { get; init; }

        public DateTime Created { get; init; }

        // ContractParameters
        public ContractParameterDto? SalaryTerm { get; init; }

        public ContractParameterDto? Currency { get; init; }

        public IEnumerable<ContractParameterDto> WorkModes { get; init; } = [];

        public IEnumerable<ContractParameterDto> EmploymentTypes { get; init; } = [];

    }
}
