using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification.Services;

public interface INotificationService
{
    Task SendToAllAsync();

    Task SendToGroupAsync(string groupName);
}
