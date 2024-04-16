using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using SignalR.Common.Constants;

namespace SignalR.SelfHosted.Notifications.Services;

public class HubMiddleWare
{
    private readonly RequestDelegate _next;

    public HubMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var request = httpContext.Request;

        if (request.Path.StartsWithSegments(HubConstants.HubEndpoint, StringComparison.OrdinalIgnoreCase)
            && request.Query.TryGetValue("access_token", out var accessToken))
        {
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        await _next(httpContext);
    }

}
