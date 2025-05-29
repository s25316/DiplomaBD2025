using Domain.Features.Recruitments.ValueObjects.Ids;
using Domain.Shared.Enums;
using UseCase.Shared.Repositories.BaseEFRepository;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Shared.Repositories.Recruitments
{
    public interface IRecruitmentRepository : IRepositoryTemplate<DomainRecruitment, RecruitmentId>
    {
        Task<(HttpCode Code, string? Message)> IsValidCreateAsync(
            DomainRecruitment item,
            CancellationToken cancellationToken);
    }
}
