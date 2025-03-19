using MediatR;
using UseCase.Shared.Dictionaries.GetContractParameters.Request;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetContractParameters
{
    public class GetContractParametersHandler : IRequestHandler<GetContractParametersRequest, IEnumerable<ContractParameterDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetContractParametersHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<ContractParameterDto>> Handle(GetContractParametersRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetContractParametersAsync();
            return dictionary.Values
                .Where(x =>
                    !request.ContractParameterTypeId.HasValue ||
                    x.ContractParameterType.ContractParameterTypeId == request.ContractParameterTypeId)
                .OrderBy(x => x.ContractParameterId);
        }
    }
}
