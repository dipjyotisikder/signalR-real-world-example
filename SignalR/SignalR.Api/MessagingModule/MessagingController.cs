using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.Api.MessagingModule.Models;
using SignalR.Api.MessagingModule.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.MessagingModule;

/// <summary>
/// Represents a controller class to perform messaging.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class MessagingController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;

    /// <summary>
    /// Constructor for <see cref="MessagingController"/>.
    /// </summary>
    public MessagingController(
        IConversationService conversationService,
        IMessageService messageService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
    }

    /// <summary>
    /// Endpoint method to create a conversation.
    /// </summary>
    [HttpPost("conversations")]
    [ProducesResponseType(typeof(ConversationModel), 200)]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationModel request)
    {
        return Ok(await _conversationService.Create(request));
    }

    /// <summary>
    /// Endpoint method to get all the conversations.
    /// </summary>
    [HttpGet("conversations")]
    [ProducesResponseType(typeof(IEnumerable<ConversationModel>), 200)]
    public IActionResult GetConversations()
    {
        return Ok(_conversationService.GetAll());
    }

    /// <summary>
    /// Endpoint method to get all the audience of a conversation.
    /// </summary>
    [HttpGet("conversations/{conversationId}/audiences")]
    [ProducesResponseType(typeof(IEnumerable<ConversationAudienceModel>), 200)]
    public async Task<IActionResult> GetAudiences([FromRoute] int conversationId)
    {
        return Ok(await _conversationService.GetAudiences(conversationId));
    }

    /// <summary>
    /// Endpoint method to get all the messages of a conversation.
    /// </summary>
    [HttpGet("conversations/{conversationId}/messages")]
    [ProducesResponseType(typeof(IEnumerable<MessageModel>), 200)]
    public IActionResult GetMessages([FromRoute] int conversationId)
    {
        return Ok(_conversationService.GetMessages(conversationId));
    }

    /// <summary>
    /// Endpoint method to create a message for a conversation.
    /// </summary>
    [HttpPost("conversations/{conversationId}/messages")]
    [ProducesResponseType(typeof(MessageModel), 200)]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageModel request)
    {
        return Ok(await _messageService.Create(request));
    }
}