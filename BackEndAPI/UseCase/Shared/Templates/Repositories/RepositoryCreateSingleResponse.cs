// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public class RepositoryCreateSingleResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResponseCommandMetadata Metadata { get; init; }


        //Public Static Methods
        public static RepositoryCreateSingleResponse ValidResponse()
        {
            return new RepositoryCreateSingleResponse
            {
                Code = HttpCode.Created,
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = true,
                    Message = HttpCode.Ok.Description()
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
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = false,
                    Message = message ?? code.Description()
                },
            };
        }
    }
}
