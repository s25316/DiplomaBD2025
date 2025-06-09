// Ignore Spelling: Middlewares, Middleware, Mongo
using UseCase.Shared.Services.Authentication.Inspectors;

namespace BackEndAPI.Middlewares.Authorization
{
    public class UserAuthorizationMiddleware
    {
        // Properties
        private static readonly string _authorizationHeader = "Authorization";
        private static readonly string _bearerString = "Bearer";
        private readonly RequestDelegate _next;


        // Constructor
        public UserAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // Methods
        public async Task Invoke(
            HttpContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            var jwt = GetJwt(context);
            if (!string.IsNullOrWhiteSpace(jwt) &&
                !authenticationInspector.IsValidJwt(jwt, true))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }

        private static string? GetJwt(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(
                _authorizationHeader,
                out var authorizationHeader))
            {
                return null;
            }
            var authHeaderValue = authorizationHeader.ToString();
            if (!authHeaderValue.StartsWith(
                _bearerString,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Someone wrote invalid \"{_authorizationHeader}\"");
            }
            return authHeaderValue.Substring(_bearerString.Length).Trim();
        }
    }
}
