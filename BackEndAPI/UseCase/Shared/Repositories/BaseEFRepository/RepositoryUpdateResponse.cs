// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Repositories.BaseEFRepository
{
    public sealed class RepositoryUpdateResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResultMetadata Metadata { get; init; }


        //Public Static Methods
        public static RepositoryUpdateResponse ValidResponse()
        {
            return new RepositoryUpdateResponse
            {
                Code = HttpCode.Ok,
                Metadata = new ResultMetadata
                {
                    IsCorrect = true,
                    Message = HttpCode.Ok.Description()
                },
            };
        }

        public static RepositoryUpdateResponse InvalidResponse(
            HttpCode code,
            string? message = null)
        {
            return new RepositoryUpdateResponse
            {
                Code = code,
                Metadata = new ResultMetadata
                {
                    IsCorrect = false,
                    Message = message ?? code.Description()
                },
            };
        }
    }
}
