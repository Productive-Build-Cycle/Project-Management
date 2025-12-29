using ProjectManagement.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProjectManagement.Api.Middlewares;

public sealed class ExceptionHandlerMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ProjectNotFoundException ex =>
                (HttpStatusCode.NotFound, ex.Message),

            ArgumentException ex =>
                (HttpStatusCode.BadRequest, ex.Message),

            InvalidOperationException ex =>
                (HttpStatusCode.BadRequest, ex.Message),

            _ =>
                (HttpStatusCode.InternalServerError,
                 "An unexpected error occurred.")
        };

        await WriteErrorResponseAsync(context, statusCode, message);
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new ErrorResponse(
            (int)statusCode,
            message),
            JsonOptions);

        await context.Response.WriteAsync(payload);
    }

    private sealed record ErrorResponse(
        int StatusCode,
        string Message);
}
