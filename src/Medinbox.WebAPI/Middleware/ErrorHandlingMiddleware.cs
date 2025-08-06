using System.Net;
using System.Text.Json;
using Medinbox.WebAPI.Models;

namespace Medinbox.WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest &&
                        context.Items.ContainsKey("ModelStateErrors"))
                    {
                        var errors = context.Items["ModelStateErrors"] as List<string>;

                        var response = new ApiResponse<List<string>>
                        {
                            ResultCode = 400,
                            Message = "Erreur de validation",
                            Data = errors
                        };

                        var json = JsonSerializer.Serialize(response);
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(json);
                    }

                    return Task.CompletedTask;
                });

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                var response = new ApiResponse<string>
                {
                    ResultCode = 500,
                    Message = "An internal error has occurred.",
                    Data = ex.Message 
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
