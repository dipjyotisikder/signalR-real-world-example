using Microsoft.AspNetCore.Mvc;
using SignalR.SelfHosted.Messages.Models;
using SignalR.SelfHosted.Messages.Services;

namespace SignalR.SelfHosted.Notification;
[ApiController]
[Route("[controller]")]
public class MessagingController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public MessagingController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost("conversations")]
    public IActionResult CreateGroups([FromBody] CreateConversationRequest request)
    {
        return Ok(_conversationService.Create(request));
    }

    [HttpGet("conversations")]
    public IActionResult GetGroups()
    {
        return Ok(_conversationService.GetAll());
    }

    [HttpGet("conversations/{conversationId}/audiences")]
    public IActionResult GetAudiences([FromRoute] int conversationId)
    {
        return Ok(_conversationService.GetAudiences(conversationId));
    }
}