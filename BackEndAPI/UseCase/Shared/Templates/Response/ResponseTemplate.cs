namespace UseCase.Shared.Templates.Response
{
    public class ResponseTemplate<T> : ResponseMetaData where T : class
    {
        public required T Result { get; init; }
    }
}

