using SignalR.SelfHosted.Notifications.Services;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Notification.Services;

public interface IHubService
{
    Task SendToAllAsync<T>(HubEventName eventName, T payload);

    Task SendToGroupsAsync<T>(string[] groups, HubEventName eventName, T payload);
}
