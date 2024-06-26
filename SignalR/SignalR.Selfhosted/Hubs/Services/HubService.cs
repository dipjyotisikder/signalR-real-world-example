using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.Hubs.Services;

/// <summary>
/// Represents the implementation class for <see cref="IHubService"/>.
/// </summary>
public class HubService : IHubService
{
    private readonly IHubContext<ApplicationHub> _hubContext;

    public HubService(IHubContext<ApplicationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <inheritdoc/>
    public Task SendToAllAsync<T>(HubEventName eventName, T payload)
    {
        return _hubContext.Clients.All.SendAsync(eventName.Value, payload);
    }

    /// <inheritdoc/>
    public Task SendToGroupsAsync<T>(IEnumerable<string> groups, HubEventName eventName, T payload)
    {
        return _hubContext.Clients.Groups(groups).SendAsync(eventName.Value, payload);
    }
}
