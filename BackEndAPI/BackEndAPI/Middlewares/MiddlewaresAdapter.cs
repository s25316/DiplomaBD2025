// Ignore Spelling: Middlewares, Middleware
using BackEndAPI.Middlewares.Authorization;

namespace BackEndAPI.Middlewares
{
    public static class MiddlewaresAdapter
    {
        public static IApplicationBuilder UseUserAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserAuthorizationMiddleware>();
        }
    }
}
