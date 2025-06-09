using UseCase.Shared.Repositories.BaseEFRepository;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.Repositories
{
    public interface IFaqRepository
    {
        Task CreateAsync(
            string question,
            string answer,
            CancellationToken cancellationToken);

        Task<RepositoryUpdateResponse> UpdateAsync(
            Guid id,
            string answer,
            CancellationToken cancellationToken);

        Task<RepositoryRemoveResponse> RemoveAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
