using UseCase.Shared.Templates.Repositories;
using DomainLogin = Domain.Shared.ValueObjects.Emails.Email;
using DomainPerson = Domain.Features.People.Aggregates.Person;
using DomainPersonId = Domain.Features.People.ValueObjects.Ids.PersonId;

namespace UseCase.Roles.Users.Repositories
{
    public interface IPersonRepository
    {
        Task<RepositoryCreateSingleResponse> CreateAsync(
           DomainPerson item,
           CancellationToken cancellationToken);

        Task<RepositorySelectResponse<DomainPerson>> GetAsync(
           DomainPersonId id,
           CancellationToken cancellationToken);

        Task<RepositorySelectResponse<DomainPerson>> GetAsync(
           DomainLogin login,
           CancellationToken cancellationToken);

        Task<RepositoryRemoveResponse> RemoveAsync(
           DomainPerson item,
           CancellationToken cancellationToken);

        Task<RepositoryUpdateResponse> UpdateAsync(
           DomainPerson item,
           CancellationToken cancellationToken);
    }
}
