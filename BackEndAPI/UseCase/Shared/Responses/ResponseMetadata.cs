using Domain.Shared.Enums;

namespace UseCase.Shared.Responses
{
    public class ResponseMetaData
    {
        public required HttpCode HttpCode { get; init; }
    }
}
