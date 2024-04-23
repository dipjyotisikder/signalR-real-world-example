using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Hubs.Services;

public interface IHubService
{
    Task SendToAllAsync<T>(HubEventName eventName, T payload);

    Task SendToGroupsAsync<T>(IEnumerable<string> groups, HubEventName eventName, T payload);
}
