using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PyroCloud.Core.Domain.Common;
using PyroCloud.Core.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace PyroCloud.Shared.Infrastructure.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error no controlado en la aplicación.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Ha ocurrido un error interno en el servidor.";
            var errors = new List<string>();

            switch (exception)
            {
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "No tienes autorización para realizar esta acción.";
                    break;

                case ArgumentException:
                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case NotImplementedException:
                    statusCode = HttpStatusCode.NotImplemented;
                    message = "La funcionalidad solicitada no está implementada.";
                    break;

                case TimeoutException:
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = "La solicitud ha excedido el tiempo de espera.";
                    break;

                case UserFriendlyException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
#if DEBUG
                    errors.Add(exception.ToString());
#endif
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.Fail(message, errors.Any() ? errors : null);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonResult = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(jsonResult);
        }
    }
}
