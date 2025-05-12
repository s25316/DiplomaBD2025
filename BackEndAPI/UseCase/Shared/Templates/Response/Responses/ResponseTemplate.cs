using UseCase.Shared.Responses;

namespace UseCase.Shared.Templates.Response.Responses
{
    public class ResponseTemplate<T> : ResponseMetaData where T : class
    {
        public required T Result { get; init; }
    }
}

