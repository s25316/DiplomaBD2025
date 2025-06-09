// Ignore Spelling: Middlewares, Middleware
using BackEndAPI.Middlewares.Authorization;
using BackEndAPI.Middlewares.GlobalErrorHandling;

namespace BackEndAPI.Middlewares
{
    public static class MiddlewaresAdapter
    {
        public static IApplicationBuilder UseUserAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserAuthorizationMiddleware>();
        }

        public static IApplicationBuilder UseGlobalErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}
