using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Pipelines.Sockets.Unofficial.Arenas;

namespace fsp.lib.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var User = context.Items["User"];
        var IPAddress = context.Connection.RemoteIpAddress;
        var Scheme = context.Request.Scheme;
        var Host = context.Request.Host.ToString();
        var Path = context.Request.Path;           
        
        var routes = context.Request.RouteValues;
        var sbRoutes = new StringBuilder();
        foreach(var route in routes )
        {
            if (sbRoutes.Length > 0) sbRoutes.Append(", ");
            sbRoutes.Append( $"{route.Key}-{route.Value}");
        }

        var UserAgent = context.Request.Headers["User-Agent"];

        _logger.LogInformation($"{User}█{IPAddress}█{Scheme}://{Host}{Path}█{sbRoutes}█{UserAgent}");        

        await _next(context);
    }
}