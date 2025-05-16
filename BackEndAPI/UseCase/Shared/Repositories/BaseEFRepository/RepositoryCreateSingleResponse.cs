// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Repositories.BaseEFRepository
{
    public class RepositoryCreateSingleResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResultMetadata Metadata { get; init; }


        //Public Static Methods
        public static RepositoryCreateSingleResponse ValidResponse()
        {
            return new RepositoryCreateSingleResponse
            {
                Code = HttpCode.Created,
                Metadata = new ResultMetadata
                {
                    IsCorrect = true,
                    Message = HttpCode.Created.Description()
                },
            };
        }

        public static RepositoryCreateSingleResponse InvalidResponse(
            HttpCode code,
            string? message = null)
        {
            return new RepositoryCreateSingleResponse
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
