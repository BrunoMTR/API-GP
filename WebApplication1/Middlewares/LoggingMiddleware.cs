using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace Presentation.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continua pipeline
                await _next(context);

                // Determina nível baseado no StatusCode
                if (context.Response.StatusCode >= 500)
                {
                    _logger.ForContext("level", "Error")
                           .Error("Server error {StatusCode} for {Path}", context.Response.StatusCode, context.Request.Path);
                }
                else if (context.Response.StatusCode >= 400)
                {
                    _logger.ForContext("level", "Warning")
                           .Warning("Client error {StatusCode} for {Path}", context.Response.StatusCode, context.Request.Path);
                }
                else
                {
                    _logger.ForContext("level", "Information")
                           .Information("Request {Method} {Path} responded {StatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção não tratada
                _logger.ForContext("level", "Error")
                       .Error(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                throw; // mantém a exceção para o middleware padrão
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
