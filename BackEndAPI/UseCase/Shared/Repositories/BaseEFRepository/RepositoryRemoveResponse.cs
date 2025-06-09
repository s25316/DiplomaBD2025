// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Repositories.BaseEFRepository
{
    public class RepositoryRemoveResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResultMetadata Metadata { get; init; }


        // Methods
        public static RepositoryRemoveResponse ValidResponse()
        {
            return PrepareResponse(HttpCode.Ok);
        }

        public static RepositoryRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new RepositoryRemoveResponse
            {
                Code = code,
                Metadata = new ResultMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? HttpCode.Ok.Description(),
                },
            };
        }
    }
}
