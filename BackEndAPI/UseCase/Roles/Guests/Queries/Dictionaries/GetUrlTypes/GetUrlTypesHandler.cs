using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetUrlTypes.Requests;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetUrlTypes
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
            return dictionary.Values.OrderBy(type => type.UrlTypeId);
        }
    }
}
