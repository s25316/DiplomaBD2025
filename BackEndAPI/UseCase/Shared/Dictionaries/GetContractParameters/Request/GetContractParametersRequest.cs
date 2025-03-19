using MediatR;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;

namespace UseCase.Shared.Dictionaries.GetContractParameters.Request
{
    public class GetContractParametersRequest : IRequest<IEnumerable<ContractParameterDto>>
    {
        public required int? ContractParameterTypeId { get; init; }
    }
}
