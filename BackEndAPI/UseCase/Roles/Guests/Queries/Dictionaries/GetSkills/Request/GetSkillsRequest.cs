using MediatR;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSkills.Request
{
    public class GetSkillsRequest : IRequest<IEnumerable<SkillResponseDto>>
    {
        public int? SkillTypeId { get; init; } = null;
    }
}
