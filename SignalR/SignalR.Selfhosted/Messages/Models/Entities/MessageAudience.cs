using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SignalR.SelfHosted.Messages.Models.Entities;

public class MessageAudience
{
    public int MessageId { get; set; }

    public int AudienceUserId { get; set; }

    public bool Seen { get; set; } = false;
}

public class MessageAudienceConfiguration : IEntityTypeConfiguration<MessageAudience>
{
    public void Configure(EntityTypeBuilder<MessageAudience> builder)
    {
        builder.HasKey(x => new
        {
            x.MessageId,
            x.AudienceUserId
        });
    }
}