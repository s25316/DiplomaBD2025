using Domain.Features.People.ValueObjects.Ids;

namespace UseCase.Shared.Templates.Repositories
{
    public interface IRepositoryTemplate<T, TId>
        where T : class, new()
        where TId : class
    {
        Task<RepositoryCreateResponse<T>> CreateAsync(
            PersonId personId,
            IEnumerable<T> items,
            CancellationToken cancellationToken);


        Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            T item,
            CancellationToken cancellationToken);

        Task<RepositorySelectResponse<T>> GetAsync(
            PersonId personId,
            TId id,
            CancellationToken cancellationToken);

        Task<RepositoryRemoveResponse> RemoveAsync(
           PersonId personId,
           T item,
           CancellationToken cancellationToken);
    }
}
