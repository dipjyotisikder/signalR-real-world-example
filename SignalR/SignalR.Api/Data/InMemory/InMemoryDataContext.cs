﻿using Microsoft.EntityFrameworkCore;
using SignalR.Api.MessagingModule.Models.Entities;
using SignalR.Api.UserModule.Models.Entities;
using System.Threading.Tasks;

namespace SignalR.Api.Data.InMemory;

/// <summary>
/// Represents the Database context class handle data.
/// </summary>
public class InMemoryDataContext : DbContext, IDataContext
{
    /// <summary>
    /// Constructor for <see cref="InMemoryDataContext"/>.
    /// </summary>
    public InMemoryDataContext(DbContextOptions<InMemoryDataContext> options) : base(options)
    {

    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public DbSet<User> Users { get; set; }

    /// <inheritdoc/>
    public DbSet<Token> Tokens { get; set; }

    /// <inheritdoc/>
    public DbSet<Conversation> Conversations { get; set; }

    /// <inheritdoc/>
    public DbSet<ConversationAudience> ConversationAudiences { get; set; }

    /// <inheritdoc/>
    public DbSet<Message> Messages { get; set; }

    /// <inheritdoc/>
    public DbSet<MessageAudience> MessageAudiences { get; set; }

    /// <inheritdoc/>
    public Task<int> SaveChangesAsync() => base.SaveChangesAsync(cancellationToken: default);
}
