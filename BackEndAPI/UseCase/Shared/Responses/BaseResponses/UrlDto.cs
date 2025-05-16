// Ignore Spelling: Dto
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;

namespace UseCase.Shared.Responses.BaseResponses
{
    public class UrlDto
    {
        public required string Value { get; init; }
        public required UrlTypeDto UrlType { get; init; }
    }
}
