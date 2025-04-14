// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public sealed class RepositorySelectResponse<T> where T : class, new()
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required T Item { get; init; }
        public required ResponseCommandMetadata Metadata { get; init; }


        // Methods
        public static RepositorySelectResponse<T> ValidResponse(T value)
        {
            return new RepositorySelectResponse<T>
            {
                Code = HttpCode.Ok,
                Item = value,
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = false,
                    Message = HttpCode.Ok.Description(),
                }
            };
        }

        public static RepositorySelectResponse<T> InvalidResponse(
            HttpCode code,
            string? message = null)
        {
            return new RepositorySelectResponse<T>
            {
                Code = code,
                Item = new T(),
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = false,
                    Message = message ?? code.Description(),
                }
            };
        }
    }
}
