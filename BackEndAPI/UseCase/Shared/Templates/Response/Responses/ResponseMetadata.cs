using Domain.Shared.Enums;

namespace UseCase.Shared.Templates.Response.Responses
{
    public class ResponseMetaData
    {
        public required HttpCode HttpCode { get; init; }
    }
}
