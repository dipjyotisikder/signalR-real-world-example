using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace SignalR.Api.MessagingModule.Models.Entities;

/// <summary>
/// Represents the class for Conversation object.
/// </summary>
public class Conversation
{
    /// <summary>
    /// Gets or sets <see cref="Id"/>.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Title"/>.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets <see cref="CreatorUserId"/>.
    /// </summary>
    public int CreatorUserId { get; set; }

    /// <summary>
    /// Gets or sets <see cref="CreatedAt"/>.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DB configuration for <see cref="Conversation"/> entity.
/// </summary>
public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
