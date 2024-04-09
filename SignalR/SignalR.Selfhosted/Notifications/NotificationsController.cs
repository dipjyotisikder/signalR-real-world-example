using Microsoft.AspNetCore.Mvc;
using SignalR.SelfHosted.Notification.Models;
using SignalR.SelfHosted.Notification.Services;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification;
[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IHubService _hubService;

    public NotificationsController(INotificationService notificationService, IHubService hubService)
    {
        _notificationService = notificationService;
        _hubService = hubService;
    }

    [HttpPost("groups")]
    public IActionResult CreateGroups([FromBody] CreateGroupRequest request)
    {
        return Ok(_hubService.CreateGroup(request.GroupName));
    }

    [HttpGet("groups")]
    public IActionResult GetGroups()
    {
        return Ok(_hubService.GetGroups());
    }

    [HttpPost("send/all")]
    public async Task<IActionResult> SendAll()
    {
        await _notificationService.SendToAllAsync();
        return Ok("Sent to all connected clients.");
    }

    [HttpPost("send/{groupName}")]
    public async Task<IActionResult> SendToGroup([FromRoute] string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            return BadRequest("Invalid/empty group name.");
        }

        await _notificationService.SendToGroupAsync(groupName);
        return Ok($"Sent to all connected clients of {groupName} group.");
    }
}