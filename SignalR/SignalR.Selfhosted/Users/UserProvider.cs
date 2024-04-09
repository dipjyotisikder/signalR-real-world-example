using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;

namespace SignalR.SelfHosted.Users;

/// <summary>
/// Class to store users.
/// </summary>
public static class UserProvider
{
    /// <summary>
    /// All the users.
    /// </summary>
    public static HashSet<User> Users { get; set; } = new HashSet<User>();
}
