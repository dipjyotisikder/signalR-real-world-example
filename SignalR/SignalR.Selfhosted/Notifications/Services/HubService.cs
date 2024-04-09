using System.Collections.Generic;

namespace SignalR.SelfHosted.Notification.Services
{
    public class HubService : IHubService
    {
        public HashSet<string> CreateGroup(string groupName)
        {
            ConnectionHandler.Groups.Add(groupName);
            return GetGroups();
        }

        public HashSet<string> GetGroups()
        {
            return ConnectionHandler.Groups;
        }
    }
}
