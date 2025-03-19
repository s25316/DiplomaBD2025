using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public sealed class RepositoryCreateResponse<T> where T : class
    {
        public required HttpCode Code { get; init; }
        public required Dictionary<T, ResponseCommandMetadata> Dictionary { get; init; }
    }
}
