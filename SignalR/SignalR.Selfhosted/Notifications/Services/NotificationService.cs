using Microsoft.AspNetCore.SignalR;
using SignalR.Common.Constants;
using SignalR.Common.Models;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendToAllAsync()
    {
        return _hubContext.Clients.All
            .SendCoreAsync(Constants.NotificationCreatedEvent,
                new object[]{
                    new NotificationMessageModel
                    {
                        Id = 1,
                        Content = "Some Content",
                        Title = "Some Title"
                    }
                });
    }

    public Task SendToGroupAsync(string groupName)
    {
        return _hubContext.Clients
            .Groups(groupName)
            .SendCoreAsync(Constants.NotificationCreatedEvent,
                new object[]{
                    new NotificationMessageModel
                    {
                        Id = 1,
                        Content = "Some Content",
                        Title = "Some Title"
                    }
                });
    }
}
