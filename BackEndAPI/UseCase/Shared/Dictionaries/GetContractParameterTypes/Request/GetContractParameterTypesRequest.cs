using MediatR;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;

namespace UseCase.Shared.Dictionaries.GetContractParameterTypes.Request
{
    public class GetContractParameterTypesRequest : IRequest<IEnumerable<ContractParameterTypeDto>>
    {
    }
}
