using System.Net;
using FastDeliveryApi.Exceptions;
using Newtonsoft.Json;

namespace FastDeliveryApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, $"Something went gront while processing {context.Request.Path}");
            await HandleExceptionAsyn(context, ex);
        }
    }

    private Task HandleExceptionAsyn(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            ErrorType = "Failure",
            ErrorMessage = ex.Message
        };

        switch (ex)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "Not Found";
                break;
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                errorDetails.ErrorType = "Bad Request";
                break;
            case UniqueEmailException uniqueEmailException:
                statusCode = HttpStatusCode.BadRequest;
                errorDetails.ErrorType = "Bad Request";
                break;
            case NotModifiedException notModifiedException:
                statusCode = HttpStatusCode.NotModified;
                errorDetails.ErrorType = "Not Modified";
                break;
            case NoContentException noContentException:
                statusCode = HttpStatusCode.NoContent;
                errorDetails.ErrorType = "No Content";
                break;
            default:
                break;
        }

        string response = JsonConvert.SerializeObject(errorDetails);
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(response);
    }

    public class ErrorDetails
    {
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}