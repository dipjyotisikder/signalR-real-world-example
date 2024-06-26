using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using SignalR.Api.Constants;

namespace SignalR.Api.Hubs.Services;

/// <summary>
/// Represents the request MiddleWare to handle access-token.
/// </summary>
public class HubMiddleWare
{
    private readonly RequestDelegate _next;

    public HubMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Represents the method to be invoked during request.
    /// </summary>
    /// <param name="httpContext">Request context.</param>
    /// <returns>A completed task.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        var request = httpContext.Request;

        if (request.Path.StartsWithSegments(HubConstants.HUB_ENDPOINT, StringComparison.OrdinalIgnoreCase)
            && request.Query.TryGetValue("access_token", out var accessToken))
        {
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        await _next(httpContext);
    }

}
