using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace SignalR.Api.MessagingModule.Models.Entities;

/// <summary>
/// Represents the class for <see cref="Message"/> object.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets <see cref="Id"/>.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Text"/>.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets <see cref="ConversationId"/>.
    /// </summary>
    public int ConversationId { get; set; }

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
/// DB configuration for <see cref="Message"/> entity.
/// </summary>
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    ///<inheritdoc/>
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
