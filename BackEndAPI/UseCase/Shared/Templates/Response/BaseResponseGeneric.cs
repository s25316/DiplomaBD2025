namespace UseCase.Shared.Templates.Response
{
    public class BaseResponseGeneric<T> : BaseResponse
    {
        public required T Item { get; init; }
    }
}
