// Ignore Spelling: Middlewares, Middleware, Mongo
using System.Security.Claims;
using UseCase.MongoDb;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace BackEndAPI.Middlewares.Authorization
{
    public class UserAuthorizationMiddleware
    {
        private static readonly string _authorizationHeader = "Authorization";
        private readonly RequestDelegate _next;


        // Constructor
        public UserAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IMongoDbService mongoService,
            IAuthenticationInspectorService authenticationInspector)
        {

            context.Items["IsAdmin"] = null;
            if (context.Request.Headers.TryGetValue(_authorizationHeader, out var authorizationHeader))
            {
                var jwt = authorizationHeader
                    .ToString()
                    .Replace("Bearer", "")
                    .Trim();

                if (!authenticationInspector.IsValidJwt(jwt, true))
                {
                    RemoveHeader(context, _authorizationHeader);
                }

                var personId = authenticationInspector.GetClaimsName(jwt, true)
                    ?? throw new Exception("Something Changed in JWT Generation");

                var userMiddlewareData = await mongoService.GetUserMiddlewareDataAsync(
                    Guid.Parse(personId),
                    jwt,
                    CancellationToken.None);

                if (userMiddlewareData.HasRemoved ||
                    userMiddlewareData.HasBlocked ||
                    userMiddlewareData.HasLogOut)
                {
                    RemoveHeader(context, _authorizationHeader);

                    Console.WriteLine(context.Request.Headers.ContainsKey(_authorizationHeader));
                }
                if (userMiddlewareData.IsAdmin)
                {
                    context.Items["IsAdmin"] = true;
                }
            }
            await _next(context);
        }

        private static void RemoveHeader(HttpContext context, string header)
        {
            if (context.Request.Headers.ContainsKey(header))
            {
                context.Request.Headers.Remove(header);
                context.User = new ClaimsPrincipal(new ClaimsIdentity());
            }
        }
    }
}
