using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSkillTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSkillTypes
{
    public class GetSkillTypesHandler : IRequestHandler<GetSkillTypesRequest, IEnumerable<SkillTypeDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetSkillTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<SkillTypeDto>> Handle(GetSkillTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetSkillTypesAsync();
            return dictionary.Values.OrderBy(value => value.SkillTypeId);
        }
    }
}
