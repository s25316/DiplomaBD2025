// Ignore Spelling: redis

using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetEmploymentTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetEmploymentTypes
{
    class GetEmploymentTypesHandler : IRequestHandler<GetEmploymentTypesRequest, IEnumerable<EmploymentTypeDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetEmploymentTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<EmploymentTypeDto>> Handle(GetEmploymentTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetEmploymentTypesAsync();
            return dictionary.Values.OrderBy(value => value.EmploymentTypeId);
        }
    }
}
