using Domain.Shared.Enums;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Shared.Repositories.Recruitments
{
    public interface IRecruitmentRepository
    {
        Task<(HttpCode Code, string? Message)> IsValidCreateAsync(
            DomainRecruitment item,
            CancellationToken cancellationToken);

        Task CreateAsync(
            DomainRecruitment item,
            CancellationToken cancellationToken);
    }
}
