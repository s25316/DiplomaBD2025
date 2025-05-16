// Ignore Spelling: Metadata
using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Responses.ItemResponse
{
    public class ResultMetadataResponse : ItemResponse<ResultMetadata>
    {
        // Methods
        public static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new ResultMetadataResponse
            {
                HttpCode = code,
                Result = new ResultMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description(),
                }
            };
        }
    }
}
