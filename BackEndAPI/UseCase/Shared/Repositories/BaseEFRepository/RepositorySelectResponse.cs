// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Repositories.BaseEFRepository
{
    public sealed class RepositorySelectResponse<T> where T : class, new()
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required T Item { get; init; }
        public required ResultMetadata Metadata { get; init; }


        // Methods
        public static RepositorySelectResponse<T> ValidResponse(T value)
        {
            return new RepositorySelectResponse<T>
            {
                Code = HttpCode.Ok,
                Item = value,
                Metadata = new ResultMetadata
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
                Metadata = new ResultMetadata
                {
                    IsCorrect = false,
                    Message = message ?? code.Description(),
                }
            };
        }
    }
}
