using Microsoft.AspNetCore.Http;
using Puush.Attributes;

namespace Puush.Middleware;

public sealed class PuushAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requiresAuth = endpoint?.Metadata.GetMetadata<PuushAuthorizeAttribute>() != null;

        if (!requiresAuth)
        {
            await next(context);
            return;
        }

        // TODO: replace this with real auth logic
        // if (!context.Response.HasStarted)
        // {
        //     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //     context.Response.ContentType = "text/plain";
        //     await context.Response.WriteAsync("Unauthorized");
        // }
        
        await next(context);
    }
}