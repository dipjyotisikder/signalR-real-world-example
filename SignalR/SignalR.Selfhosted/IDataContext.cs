using Microsoft.EntityFrameworkCore;
using SignalR.Api.MessagingModule.Models.Entities;
using SignalR.Api.UserModule.Models.Entities;
using System.Threading.Tasks;

namespace SignalR.Api;

/// <summary>
/// Class to store data.
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// Provides all the user data.
    /// </summary>
    DbSet<User> Users { get; set; }

    /// <summary>
    /// Provides all the conversation data.
    /// </summary>
    DbSet<Conversation> Conversations { get; set; }

    /// <summary>
    /// Provides all the conversation audience data.
    /// </summary>
    DbSet<ConversationAudience> ConversationAudiences { get; set; }

    /// <summary>
    /// Provides all the messages.
    /// </summary>
    DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Provides all the message audiences.
    /// </summary>
    DbSet<MessageAudience> MessageAudiences { get; set; }

    /// <summary>
    /// Provides all the tokens.
    /// </summary>
    DbSet<Token> Tokens { get; set; }

    /// <summary>
    /// Represents the change tracking and save method.
    /// </summary>
    /// <returns>Indicator if change occurred.</returns>
    Task<int> SaveChangesAsync();
}
