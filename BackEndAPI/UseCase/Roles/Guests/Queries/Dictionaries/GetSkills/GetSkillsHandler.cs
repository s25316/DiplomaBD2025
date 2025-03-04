using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSkills.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSkills
{
    public class GetSkillsHandler : IRequestHandler<GetSkillsRequest, IEnumerable<SkillResponseDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetSkillsHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<SkillResponseDto>> Handle(GetSkillsRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetSkillsAsync();
            if (request.SkillTypeId.HasValue)
            {
                return dictionary.Values
                    .Where(skill => skill.SkillType.SkillTypeId == request.SkillTypeId)
                    .OrderBy(value => value.SkillId);
            }
            return dictionary.Values.OrderBy(value => value.SkillId);
        }
    }
}
