using SignalR.SelfHosted.Messages.Models;
using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;

namespace SignalR.SelfHosted;

/// <summary>
/// Class to store data.
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// Provides all the user data.
    /// </summary>
    List<User> Users { get; set; }

    /// <summary>
    /// Provides all the conversation data.
    /// </summary>
    List<Conversation> Conversations { get; set; }

    /// <summary>
    /// Provides all the conversation audience data.
    /// </summary>
    List<ConversationAudience> ConversationAudiences { get; set; }

    /// <summary>
    /// Provides all the messages.
    /// </summary>
    List<Message> Messages { get; set; }

    /// <summary>
    /// Provides all the message audiences.
    /// </summary>
    List<MessageAudience> MessageAudiences { get; set; }

    /// <summary>
    /// Provides all the tokens.
    /// </summary>
    List<Token> Tokens { get; set; }
}
