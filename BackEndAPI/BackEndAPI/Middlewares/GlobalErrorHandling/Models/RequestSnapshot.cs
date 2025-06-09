namespace BackEndAPI.Middlewares.GlobalErrorHandling.Models
{
    public class RequestSnapshot
    {
        public required string Method { get; init; }
        public required string Path { get; init; }
        public required string QueryString { get; init; }
        public required Dictionary<string, string> Headers { get; init; } = [];
        public required string Body { get; init; }
        public required string Host { get; init; }
        public required string Scheme { get; init; }
        public required string ClientIpAddress { get; init; }
    }
}
