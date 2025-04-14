// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public class RepositoryRemoveResponse
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required ResponseCommandMetadata Metadata { get; init; }


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
                Code = HttpCode.Ok,
                Metadata = new ResponseCommandMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? HttpCode.Ok.Description(),
                },
            };
        }
    }
}
