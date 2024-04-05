using System.Collections.Generic;

namespace SignalR.SelfHosted.Notification.Services
{
    public interface IHubService
    {
        HashSet<string> GetGroups();

        HashSet<string> CreateGroup(string groupName);
    }
}
