using Microsoft.AspNetCore.SignalR;
using SignalR.SelfHosted.Users.Services;
using System;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification;
public class ApplicationHub : Hub
{
    private readonly IUserService _userService;
    public ApplicationHub(IUserService userService)
    {
        _userService = userService;
    }

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
    /// Automatically called when connected.
    /// </summary>
    /// <returns>A completed Task.</returns>
    public override Task OnConnectedAsync()
    {
        var userId = 1;

        _userService.OnLineUser(true, userId);
        return base.OnConnectedAsync();
    }

    /// <summary>
    /// Automatically called when disconnected.
    /// </summary>
    /// <param name="exception">Exception generated from connection.</param>
    /// <returns>A completed Task.</returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
        return Task.WhenAll(
            Clients.All.SendAsync("ExceptionOccured", exception?.Message),
            base.OnDisconnectedAsync(exception));
    }
}
