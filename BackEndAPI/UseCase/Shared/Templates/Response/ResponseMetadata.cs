using Domain.Shared.Enums;

namespace UseCase.Shared.Templates.Response
{
    public class ResponseMetaData
    {
        public required bool IsCorrect { get; init; }
        public required HttpCode HttpCode { get; init; }
    }
}
