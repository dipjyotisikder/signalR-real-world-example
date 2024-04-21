using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SignalR.SelfHosted.Messages.Models.Entities;

public class ConversationAudience
{
    public int ConversationId { get; set; }

    public int AudienceUserId { get; set; }
}

public class ConversationAudienceConfiguration : IEntityTypeConfiguration<ConversationAudience>
{
    public void Configure(EntityTypeBuilder<ConversationAudience> builder)
    {
        builder.HasKey(x => new
        {
            x.ConversationId,
            x.AudienceUserId
        });
    }
}
