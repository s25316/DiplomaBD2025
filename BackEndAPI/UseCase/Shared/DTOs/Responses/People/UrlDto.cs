// Ignore Spelling: Dto
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;

namespace UseCase.Shared.DTOs.Responses.People
{
    public class UrlDto
    {
        public required string Value { get; init; }
        public required UrlTypeDto UrlType { get; init; }
    }
}
