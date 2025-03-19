using MediatR;
using UseCase.Shared.Dictionaries.GetUrlTypes.Request;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetUrlTypes
{
    public class GetUrlTypesHandler : IRequestHandler<GetUrlTypesRequest, IEnumerable<UrlTypeDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetUrlTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<UrlTypeDto>> Handle(GetUrlTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetUrlTypesAsync();
            return dictionary.Values.OrderBy(x => x.UrlTypeId);
        }
    }
}
