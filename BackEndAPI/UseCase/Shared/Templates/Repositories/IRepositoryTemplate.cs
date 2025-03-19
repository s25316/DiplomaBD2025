using Domain.Features.People.ValueObjects;

namespace UseCase.Shared.Templates.Repositories
{
    public interface IRepositoryTemplate<T> where T : class
    {
        Task<RepositoryCreateResponse<T>> CreateAsync(
            PersonId personId,
            IEnumerable<T> items,
            CancellationToken cancellationToken);
    }
}
