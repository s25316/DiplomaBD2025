using Domain.Shared.Enums;

namespace Domain.Shared.Templates
{
    public class TemplateException : Exception
    {
        // Properties
        public HttpCode Code { get; init; }

        // Constructor
        public TemplateException(string? message, HttpCode code) : base(message)
        {
            Code = code;
        }
    }
}
