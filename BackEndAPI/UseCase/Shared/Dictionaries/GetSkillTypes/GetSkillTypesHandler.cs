using MediatR;
using UseCase.Shared.Dictionaries.GetSkillTypes.Request;
using UseCase.Shared.Dictionaries.GetSkillTypes.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetSkillTypes
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
            return dictionary.Values.OrderBy(x => x.SkillTypeId);
        }
    }
}
