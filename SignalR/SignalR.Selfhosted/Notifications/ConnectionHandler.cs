using System.Collections.Generic;

namespace SignalR.SelfHosted.Notification;

/// <summary>
/// Class to store connections.
/// </summary>
public static class ConnectionHandler
{
    /// <summary>
    /// All the live connection IDs.
    /// </summary>
    public static HashSet<string> ConnectedIds { get; set; } = new HashSet<string>();

    /// <summary>
    /// All the groups.
    /// </summary>
    public static HashSet<string> Groups { get; set; } = new HashSet<string> { "ChatRoom1", "ChatRoom2" };
}
