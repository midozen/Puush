using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Puush.Infrastructure.Security.Attributes;
using Puush.Infrastructure.Services;

namespace Puush.Infrastructure.Security.Middleware;

public sealed class PuushAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        var endpoint = context.GetEndpoint();
        var requiresAuth = endpoint?.Metadata.GetMetadata<PuushAuthorizeAttribute>() != null;
        
        if (!requiresAuth)
        {
            await next(context);
            return;
        }
        
        // 1. Check if method is POST
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await RejectAsync(context, 405, "Method Not Allowed");
            return;
        }

        // 2. Check if request has form data
        if (!context.Request.HasFormContentType)
        {
            await RejectAsync(context, 405, "Bad Request");
            return;
        }
        
        // 3. Get API Key
        context.Request.EnableBuffering();
        var form = await context.Request.ReadFormAsync();
        context.Request.Body.Position = 0;

        var apiKey = form["k"].FirstOrDefault();

        if (string.IsNullOrEmpty(apiKey))
        {
            await RejectAsync(context, 401, "Unauthorized");
            return;
        }
        
        // 4. Validate API Key
        var session = await sessionService.ValidateSessionAsync(apiKey);
        if (session == null)
        {
            await RejectAsync(context, 401, "Unauthorized");
            return;
        }
        
        // 5. Create claims and set user
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.AccountId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "PuushAuth");
        context.User = new ClaimsPrincipal(identity);
        
        await next(context);
    }

    private static async Task RejectAsync(HttpContext context, int statusCode, string message)
    {
        if (context.Response.HasStarted)
            return;
        
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "text/plain; charset=utf-8";
        
        await context.Response.WriteAsync(message);
    }
}
