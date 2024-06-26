using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SignalR.Api.MessagingModule.Models.Entities;

/// <summary>
/// Represents the class for <see cref="ConversationAudience"/> object.
/// </summary>
public class ConversationAudience
{
    /// <summary>
    /// Gets or sets <see cref="ConversationId"/>.
    /// </summary>
    public int ConversationId { get; set; }

    /// <summary>
    /// Gets or sets <see cref="AudienceUserId"/>.
    /// </summary>
    public int AudienceUserId { get; set; }
}

/// <summary>
/// DB configuration for <see cref="ConversationAudience"/> entity.
/// </summary>
public class ConversationAudienceConfiguration : IEntityTypeConfiguration<ConversationAudience>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ConversationAudience> builder)
    {
        builder.HasKey(x => new
        {
            x.ConversationId,
            x.AudienceUserId
        });
    }
}
