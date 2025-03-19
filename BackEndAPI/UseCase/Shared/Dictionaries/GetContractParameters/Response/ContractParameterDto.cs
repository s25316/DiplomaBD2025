// Ignore Spelling: Dto

using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;

namespace UseCase.Shared.Dictionaries.GetContractParameters.Response
{
    public class ContractParameterDto
    {
        public int ContractParameterId { get; init; }

        public string Name { get; init; } = null!;

        public ContractParameterTypeDto ContractParameterType { get; init; } = null!;
    }
}
