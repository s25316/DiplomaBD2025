using MediatR;
using UseCase.Shared.Dictionaries.GetSkills.Request;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetSkills
{
    public class GetSkillsHandler : IRequestHandler<GetSkillsRequest, IEnumerable<SkillDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetSkillsHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<SkillDto>> Handle(GetSkillsRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetSkillsAsync();
            return dictionary.Values.Where(x =>
                    !request.SkillTypeId.HasValue ||
                    x.SkillType.SkillTypeId == request.SkillTypeId)
                .OrderBy(x => x.SkillId);
        }
    }
}
