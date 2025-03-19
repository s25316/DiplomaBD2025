using MediatR;
using UseCase.Shared.Dictionaries.GetSkillTypes.Response;

namespace UseCase.Shared.Dictionaries.GetSkillTypes.Request
{
    public class GetSkillTypesRequest : IRequest<IEnumerable<SkillTypeDto>>
    {
    }
}
