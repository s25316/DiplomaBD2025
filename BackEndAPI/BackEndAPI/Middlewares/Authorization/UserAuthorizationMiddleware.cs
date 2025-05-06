// Ignore Spelling: Middlewares, Middleware, Mongo
using UseCase.MongoDb;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace BackEndAPI.Middlewares.Authorization
{
    public class UserAuthorizationMiddleware
    {
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
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var jwt = authorizationHeader
                    .ToString()
                    .Replace("Bearer ", "")
                    .Trim();

                if (!authenticationInspector.IsValidJwt(jwt, true))
                {
                    context.Response.StatusCode = 401;
                    return;
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
                    context.Response.StatusCode = 401;
                    return;
                }
                if (userMiddlewareData.IsAdmin)
                {
                    context.Items["IsAdmin"] = true;
                }

            }
            await _next(context);
        }
    }
}
