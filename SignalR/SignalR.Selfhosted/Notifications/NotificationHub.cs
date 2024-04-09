using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification;
public class NotificationHub : Hub
{
    /// <summary>
    /// Represents a Hub method to connect to a group.
    /// </summary>
    /// <param name="groupName">Group name.</param>
    /// <returns>A task.</returns>
    public Task JoinGroup(string groupName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Represents a Hub method to connect to a group.
    /// </summary>
    /// <param name="groupName">Group name.</param>
    /// <returns>A task.</returns>
    public Task Broadcast(object data)
    {
        return Clients.All.SendAsync("Broadcast", data);
    }

    /// <summary>
    /// Automatically called when connected.
    /// </summary>
    /// <returns>A completed Task.</returns>
    public override Task OnConnectedAsync()
    {
        ConnectionHandler.ConnectedIds.Add(Context.ConnectionId);
        return Task.WhenAll(
            Clients.All.SendAsync("ConnectedClientUpdated", ConnectionHandler.ConnectedIds.Count),
            base.OnConnectedAsync());
    }

    /// <summary>
    /// Automatically called when disconnected.
    /// </summary>
    /// <param name="exception">Exception generated from connection.</param>
    /// <returns>A completed Task.</returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (ConnectionHandler.ConnectedIds.Any(x => x == Context.ConnectionId))
            ConnectionHandler.ConnectedIds.Remove(Context.ConnectionId);

        return Task.WhenAll(
            Clients.All.SendAsync("ConnectedClientUpdated", ConnectionHandler.ConnectedIds.Count),
            Clients.All.SendAsync("ExceptionOccured", exception?.Message),
            base.OnDisconnectedAsync(exception));
    }
}
