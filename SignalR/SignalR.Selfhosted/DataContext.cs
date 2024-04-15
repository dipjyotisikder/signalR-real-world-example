using SignalR.SelfHosted.Messages.Models;
using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;

namespace SignalR.SelfHosted;

/// <summary>
/// Class to store data.
/// </summary>
public class DataContext : IDataContext
{
    /// <summary>
    /// Provides all the user data.
    /// </summary>
    public List<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Provides all the conversation data.
    /// </summary>
    public List<Conversation> Conversations { get; set; } = new List<Conversation>();

    /// <summary>
    /// Provides all the conversation audience data.
    /// </summary>
    public List<ConversationAudience> ConversationAudiences { get; set; } = new List<ConversationAudience>();

    /// <summary>
    /// Provides all the messages.
    /// </summary>
    public List<Message> Messages { get; set; } = new List<Message>();

    /// <summary>
    /// Provides all the message audiences.
    /// </summary>
    public List<MessageAudience> MessageAudiences { get; set; } = new List<MessageAudience>();
}
