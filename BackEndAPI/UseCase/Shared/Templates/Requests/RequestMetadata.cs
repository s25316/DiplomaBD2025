// Ignore Spelling: Metadata

using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UseCase.Shared.Templates.Requests
{
    public class RequestMetadata
    {
        // Properties
        public string? IP { get; init; } = null;
        public string? Browser { get; init; } = null;
        public string? Authorization { get; init; } = null;
        public IEnumerable<Claim> Claims { get; init; } = [];


        // Methods
        public static implicit operator RequestMetadata(HttpContext context)
        {
            var jwt = (string?)context.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(jwt))
            {
                jwt = jwt
                    .Replace("Bearer", "")
                    .Trim();
            }

            return new RequestMetadata
            {
                IP = context.Connection.RemoteIpAddress?.ToString(),
                Browser = context.Request.Headers["User-Agent"].ToString(),

                Authorization = jwt,
                Claims = context.User.Claims ?? [],
            };
        }
    }
}
