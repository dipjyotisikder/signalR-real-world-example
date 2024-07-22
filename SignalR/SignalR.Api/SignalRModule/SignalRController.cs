using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using SignalR.Api.Infrastructure.Services;
using SignalR.Api.SignalRModule.Models;
using System.Threading.Tasks;

namespace SignalR.Api.SignalRModule;

/// <summary>
/// Represents a controller class to communicate with Azure SignalR.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SignalRController : ControllerBase
{
    private readonly IAzureSignalRService _azureSignalRService;

    /// <summary>
    /// Constructor for <see cref="SignalRController"/>.
    /// </summary>
    /// <param name="azureSignalRService"></param>
    public SignalRController(IAzureSignalRService azureSignalRService)
    {
        _azureSignalRService = azureSignalRService;
    }

    /// <summary>
    /// Endpoint method to establish azure SignalR connection in a group.
    /// </summary>
    /// <param name="groupName">Intended group to connect.</param>
    /// <returns></returns>
    [HttpPost("negotiate")]
    [ProducesResponseType(typeof(NegotiationResponse), statusCode: 200)]
    public async Task<IActionResult> Negotiate([FromQuery] string groupName)
    {
        var result = await _azureSignalRService.AddToGroupAsync(groupName);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint method to Send a signalR message to a connection group.
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPost("{groupName}")]
    [ProducesResponseType(typeof(OkResult), statusCode: 200)]
    public async Task<IActionResult> AzureNotification([FromRoute] string groupName, [FromBody] SignalRMessageModel message)
    {
        await _azureSignalRService.SendToGroupAsync(groupName, message);
        return Ok();
    }
}