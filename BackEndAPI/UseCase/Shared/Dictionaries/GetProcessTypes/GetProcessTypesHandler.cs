using MediatR;
using UseCase.Shared.Dictionaries.GetProcessTypes.Request;
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetProcessTypes
{
    public class GetProcessTypesHandler : IRequestHandler<GetProcessTypesRequest, IEnumerable<ProcessTypeDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetProcessTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<ProcessTypeDto>> Handle(GetProcessTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetProcessTypesAsync();
            return dictionary.Values.OrderBy(x => x.ProcessTypeId);
        }
    }
}
