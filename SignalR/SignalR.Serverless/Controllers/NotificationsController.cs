using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;
using SignalR.Common;
using SignalR.Models;
using System;
using System.Threading.Tasks;

namespace SignalR.Serverless.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ServiceHubContext _serviceHubContext;

        public NotificationsController(ServiceHubContext serviceHubContext)
        {
            _serviceHubContext = serviceHubContext;
        }

        [HttpPost("negotiate")]
        public async Task<IActionResult> Negotiate([FromQuery] string groupName)
        {
            var userId = $"User.{Guid.NewGuid()}";
            var negotiationResponse = await _serviceHubContext.NegotiateAsync(new NegotiationOptions
            {
                HttpContext = HttpContext,
                UserId = userId
            });
            await _serviceHubContext.UserGroups.AddToGroupAsync(userId, groupName);

            return Ok(negotiationResponse);
        }

        [HttpPost("all")]
        public async Task<IActionResult> SendAll()
        {
            await _serviceHubContext.Clients.All
                .SendCoreAsync(Constants.NotificationCreatedEvent,
                    new object[]{
                        new NotificationMessageModel
                        {
                            Id = 1,
                            Content= "Some Content",
                            Title= "Some Title"
                        }
                    });

            return Ok("Sent to all connected clients.");
        }

        [HttpPost("{groupName}")]
        public async Task<IActionResult> AzureNotification([FromRoute] string groupName)
        {
            await _serviceHubContext.Clients
                .Group(groupName)
                .SendCoreAsync(Constants.NotificationCreatedEvent,
                    new object[]{
                        new NotificationMessageModel
                        {
                            Id = 1,
                            Content = "Some Content",
                            Title = "Some Title"
                        }
                    });

            return Ok($"Sent to all connected clients of {groupName} group.");
        }
    }
}