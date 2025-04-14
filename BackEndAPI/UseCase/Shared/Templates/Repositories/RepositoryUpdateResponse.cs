// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public sealed class RepositoryUpdateResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResponseCommandMetadata Metadata { get; init; }


        //Public Static Methods
        public static RepositoryUpdateResponse ValidResponse()
        {
            return new RepositoryUpdateResponse
            {
                Code = HttpCode.Ok,
                Metadata = new ResponseCommandMetadata
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
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = false,
                    Message = message ?? code.Description()
                },
            };
        }
    }
}
