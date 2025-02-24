namespace UseCase.Shared.Exceptions
{
    public class UseCaseLayerException : Exception
    {
        public UseCaseLayerException()
        {
        }

        public UseCaseLayerException(string? message) : base(message)
        {
        }
    }
}
