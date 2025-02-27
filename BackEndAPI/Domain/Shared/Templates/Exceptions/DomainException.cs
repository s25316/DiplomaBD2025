namespace Domain.Shared.Templates.Exceptions
{
    public class DomainException : Exception
    {
        // Properties
        public HttpCodeEnum Code { get; init; }

        // Constructor
        public DomainException(string? message, HttpCodeEnum code) : base(message)
        {
            Code = code;
        }
    }
}
