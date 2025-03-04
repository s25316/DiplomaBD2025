// Ignore Spelling: redis

using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetWorkModes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetWorkModes
{
    class GetWorkModesHandler : IRequestHandler<GetWorkModesRequest, IEnumerable<WorkModeDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetWorkModesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<WorkModeDto>> Handle(GetWorkModesRequest request, CancellationToken cancellationToken)
        {
            var dictonary = await _dictionariesRepository.GetWorkModesAsync();
            return dictonary.Values.OrderBy(value => value.WorkModeId);
        }
    }
}
