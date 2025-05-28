// Ignore Spelling: Dto
namespace UseCase.Shared.Responses.ItemResponse.FileResponses
{
    public class FileDto
    {
        public required Stream Stream { get; init; }
        public required string FileName { get; init; }
    }
}
