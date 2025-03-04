using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSkills.Request
{
    public class GetSkillsRequest : IRequest<IEnumerable<SkillResponseDto>>
    {
        public int? SkillTypeId { get; init; } = null;
    }
}
