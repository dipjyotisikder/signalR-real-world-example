using Microsoft.AspNetCore.Mvc;

namespace SignalR.SelfHosted.Notification;
[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    /*private readonly IHubService _notificationService;
    private readonly IConversationService _conversationService;

    public NotificationsController(IHubService notificationService, IConversationService hubService)
    {
        _notificationService = notificationService;
        _conversationService = hubService;
    }

    [HttpPost("groups")]
    public IActionResult CreateGroups([FromBody] CreateConversationRequest request)
    {
        return Ok(_conversationService.Create(request));
    }

    [HttpGet("groups")]
    public IActionResult GetGroups()
    {
        return Ok(_conversationService.GetAll());
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
    }*/
}