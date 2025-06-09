using BackEndAPI.Middlewares.GlobalErrorHandling.Models;
using Domain.Shared.CustomProviders;
using System.Text;
using System.Text.Json;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;

namespace BackEndAPI.Middlewares.GlobalErrorHandling
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;


        // Constructor
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            DiplomaBdContext dbContext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var request = await PrepareRequestAsync(context);
                var exception = new Ex
                {
                    Created = CustomTimeProvider.Now,
                    ExceptionType = ex.GetType().ToString(),
                    StackTrace = "",
                    Method = "",
                    Message = ex.ToString(),
                    Other = request,
                    Request = 1, //context.Request.ToJson().ToString(),
                };
                await dbContext.Exs.AddAsync(exception);
                await dbContext.SaveChangesAsync();

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;

                var responseDto = new ErrorResponse { Id = exception.ExceptionId };
                var json = JsonSerializer.Serialize(responseDto);
                await response.WriteAsync(json);
            }
        }

        private async static Task<string> PrepareRequestAsync(HttpContext context)
        {
            var request = context.Request;
            request.EnableBuffering();
            string requestBody;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;

            var snapshot = new RequestSnapshot
            {
                Method = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(h => h.Key, h => string.Join("; ", h.Value)),
                Body = requestBody,
                Host = request.Host.ToString(),
                Scheme = request.Scheme,
                ClientIpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(snapshot, options);
        }
    }
}
