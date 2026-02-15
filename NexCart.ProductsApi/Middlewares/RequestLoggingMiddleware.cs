using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NexCart.ProductsApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try { context.Request.EnableBuffering(); } catch { }

        string requestBody = string.Empty;
        try
        {
            if (context.Request.ContentLength is > 0 and < 1024 * 1024)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
        }
        catch { }

        var originalBody = context.Response.Body;
        await using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = string.Empty;
            try
            {
                if (context.Response.Body.Length > 0 && context.Response.Body.Length < 1024 * 1024)
                {
                    using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
                    responseBody = await reader.ReadToEndAsync();
                }
            }
            catch { }

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await memStream.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {Elapsed}ms. RequestBody: {RequestBody} ResponseBody: {ResponseBody}",
                context.Request.Method,
                context.Request.Path + context.Request.QueryString,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                Truncate(requestBody, 2000),
                Truncate(responseBody, 2000));
        }
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...[truncated]";
    }
}
