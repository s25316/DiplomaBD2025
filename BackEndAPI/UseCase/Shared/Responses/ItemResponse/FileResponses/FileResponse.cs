using Domain.Shared.Enums;

namespace UseCase.Shared.Responses.ItemResponse.FileResponses
{
    public class FileResponse : ItemResponse<FileDto>
    {
        // Methods
        public static FileResponse PrepareResponse(
            HttpCode code,
            FileDto? item = null)
        {
            return new FileResponse
            {
                HttpCode = code,
                Result = item,
            };
        }
    }
}
