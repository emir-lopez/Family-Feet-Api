using System.Text.Json;
using FamilyFeet.Api.Common;
using Microsoft.Data.SqlClient;

namespace FamilyFeet.Api.Middleware;

/// <summary>
/// Guarantees that EVERY error (unhandled exception, unknown route, wrong HTTP verb...)
/// is returned with the same Response envelope instead of an empty body or an HTML page.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOption = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (ShouldWriteDefaultBody(context.Response))
            {
                await WriteAsync(context, context.Response.StatusCode, GetDefaultMessage(context.Response.StatusCode));
            }
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogInformation("Request cancelled by the client: {Method} {Path}", context.Request.Method, context.Request.Path);

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 499;
            }
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Server error while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteAsync(context, StatusCodes.Status500InternalServerError,
                "A database error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteAsync(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static bool ShouldWriteDefaultBody(HttpResponse response) =>
        response.StatusCode >= StatusCodes.Status400BadRequest
        && !response.HasStarted
        && response.ContentLength is null
        && string.IsNullOrEmpty(response.ContentType);

    private static async Task WriteAsync(HttpContext context, int statusCode, string message)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = statusCode;

        var body = new Response<object>
        {
            Result = ResultCode.FromHttpStatus(statusCode),
            Msg = message
        };

        await context.Response.WriteAsJsonAsync(body, SerializerOption, context.RequestAborted);
    }

    private static string GetDefaultMessage(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "The request is invalid.",
        StatusCodes.Status401Unauthorized => "Authentication is required.",
        StatusCodes.Status403Forbidden => "You do not have permission to perform this action.",
        StatusCodes.Status404NotFound => "The requested resource was not found.",
        StatusCodes.Status405MethodNotAllowed => "The HTTP method is not allowed for this resource.",
        StatusCodes.Status415UnsupportedMediaType => "Unsupported content type. Use application/json.",
        >= 500 => "An unexpected error occurred.",
        _ => "The request could not be processed."
    };
}
