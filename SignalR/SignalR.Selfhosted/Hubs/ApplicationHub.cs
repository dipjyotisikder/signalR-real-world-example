using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.SelfHosted.Users.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Hubs;

/// <summary>
/// Represents the Application SignalR hub to real-time communication.
/// </summary>
[Authorize]
public class ApplicationHub : Hub
{
    private readonly IUserService _userService;
    public ApplicationHub(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Automatically called when connected.
    /// </summary>
    /// <returns>A completed Task.</returns>
    public override async Task OnConnectedAsync()
    {
        var claim = Context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
        bool parsed = int.TryParse(claim.Value, out int userId);
        if (parsed)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, claim.Value);
        }

        await base.OnConnectedAsync();
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

    /// <summary>
    /// Represents a invokable hub method.
    /// </summary>
    /// <param name="conversationId">Conversation Identifier.</param>
    /// <param name="isTyping">If an user is typing or not.</param>
    /// <returns>A task.</returns>
    public Task UserIsTyping(int conversationId, bool isTyping)
    {
        return _userService.TriggerUserIsTypingEvent(conversationId, isTyping);
    }
}