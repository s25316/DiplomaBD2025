using MediatR;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Request;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetContractParameterTypes
{
    public class GetContractParameterTypesHandler : IRequestHandler<GetContractParameterTypesRequest, IEnumerable<ContractParameterTypeDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetContractParameterTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<ContractParameterTypeDto>> Handle(GetContractParameterTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetContractParameterTypesAsync();
            return dictionary.Values.OrderBy(x => x.ContractParameterTypeId);
        }
    }
}
