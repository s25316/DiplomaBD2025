using MediatR;
using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.Dictionaries.GetSkills.Request
{
    public class GetSkillsRequest : IRequest<IEnumerable<SkillDto>>
    {
        public required int? SkillTypeId { get; init; }
    }
}
