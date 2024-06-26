using Microsoft.AspNetCore.Mvc;
using SignalR.Api.Infrastructure.Services;
using SignalR.Api.SignalRModule.Models;
using System.Threading.Tasks;

namespace SignalR.Api.SignalRModule;

[ApiController]
[Route("[controller]")]
public class SignalRController : ControllerBase
{
    private readonly IAzureSignalRService _azureSignalRService;

    public SignalRController(IAzureSignalRService azureSignalRService)
    {
        this._azureSignalRService = azureSignalRService;
    }

    [HttpPost("negotiate")]
    public async Task<IActionResult> Negotiate([FromQuery] string groupName)
    {
        var result = await _azureSignalRService.AddToGroupAsync(groupName);
        return Ok(result);
    }

    [HttpPost("{groupName}")]
    public async Task<IActionResult> AzureNotification([FromRoute] string groupName, [FromBody] SignalRMessageModel message)
    {
        await _azureSignalRService.SendToGroupAsync(groupName, message);
        return Ok();
    }
}