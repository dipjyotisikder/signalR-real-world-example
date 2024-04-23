using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace SignalR.SelfHosted.Messages.Models.Entities;

public class Message
{
    public int Id { get; set; }

    public string Text { get; set; }

    public int ConversationId { get; set; }

    public int CreatorUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
