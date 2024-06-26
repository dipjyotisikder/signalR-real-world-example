using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.Hubs.Services;

/// <summary>
/// Represents the interface for Hub operation.
/// </summary>
public interface IHubService
{
    /// <summary>
    /// Represents the method for sending SignalR message to all.
    /// </summary>
    /// <typeparam name="T">Message payload type.</typeparam>
    /// <param name="eventName">Hub event name.</param>
    /// <param name="payload">Message payload object.</param>
    /// <returns>Returns Task.</returns>
    Task SendToAllAsync<T>(HubEventName eventName, T payload);

    /// <summary>
    /// Represents the method for sending SignalR message to groups.
    /// </summary>
    /// <typeparam name="T">Message payload type.</typeparam>
    /// <param name="groups"></param>
    /// <param name="eventName">Hub event name.</param>
    /// <param name="payload">Message payload object.</param>
    /// <returns>Returns Task.</returns>
    Task SendToGroupsAsync<T>(IEnumerable<string> groups, HubEventName eventName, T payload);
}
