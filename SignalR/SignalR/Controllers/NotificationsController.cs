using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Constants;
using SignalR.Hubs;
using SignalR.Models;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("all")]
        public async Task<IActionResult> SendAll()
        {
            await _hubContext.Clients.All
                .SendCoreAsync(CommonConstants.NotificationCreatedEvent,
                    new object[]{
                        new SendNotificationRequest
                        {
                            Id = 1,
                            ContentTemplate = "Some Content",
                            TitleTemplate = "Some Title"
                        }
                    });

            return Ok("Sent to all connected clients.");
        }

        [HttpPost("{groupName}")]
        public async Task<IActionResult> SendToGroup([FromRoute] string groupName)
        {
            await _hubContext.Clients
                .Groups(groupName)
                .SendCoreAsync(CommonConstants.NotificationCreatedEvent,
                    new object[]{
                        new SendNotificationRequest
                        {
                            Id = 1,
                            ContentTemplate = "Some Content",
                            TitleTemplate = "Some Title"
                        }
                    });

            return Ok($"Sent to all connected clients of {groupName} group.");
        }
    }
}