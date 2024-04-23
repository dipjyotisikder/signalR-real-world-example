using Microsoft.EntityFrameworkCore;
using SignalR.SelfHosted.Messages.Models.Entities;
using SignalR.SelfHosted.Users.Models.Entities;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Data.SqLite;

/// <summary>
/// Class to store data.
/// </summary>
public class SqLiteDataContext : DbContext, IDataContext
{
    public SqLiteDataContext(DbContextOptions<SqLiteDataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TokenConfiguration());

        modelBuilder.ApplyConfiguration(new ConversationConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationAudienceConfiguration());

        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new MessageAudienceConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Provides all the user data.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Provides all the tokens.
    /// </summary>
    public DbSet<Token> Tokens { get; set; }

    /// <summary>
    /// Provides all the conversation data.
    /// </summary>
    public DbSet<Conversation> Conversations { get; set; }

    /// <summary>
    /// Provides all the conversation audience data.
    /// </summary>
    public DbSet<ConversationAudience> ConversationAudiences { get; set; }

    /// <summary>
    /// Provides all the messages.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Provides all the message audiences.
    /// </summary>
    public DbSet<MessageAudience> MessageAudiences { get; set; }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync(cancellationToken: default);
}
