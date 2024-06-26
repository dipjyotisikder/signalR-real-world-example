using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.Api.MessagingModule.Models;
using SignalR.Api.MessagingModule.Services;
using System.Threading.Tasks;

namespace SignalR.Api.MessagingModule;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MessagingController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;

    public MessagingController(IConversationService conversationService, IMessageService messageService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationModel request)
    {
        return Ok(await _conversationService.Create(request));
    }

    [HttpGet("conversations")]
    public IActionResult GetConversations()
    {
        return Ok(_conversationService.GetAll());
    }

    [HttpGet("conversations/{conversationId}/audiences")]
    public async Task<IActionResult> GetAudiences([FromRoute] int conversationId)
    {
        return Ok(await _conversationService.GetAudiences(conversationId));
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public IActionResult GetMessages([FromRoute] int conversationId)
    {
        return Ok(_conversationService.GetMessages(conversationId));
    }

    [HttpPost("conversations/{conversationId}/messages")]
    public async Task<IActionResult> GetMessages([FromBody] CreateMessageModel request)
    {
        return Ok(await _messageService.Create(request));
    }
}