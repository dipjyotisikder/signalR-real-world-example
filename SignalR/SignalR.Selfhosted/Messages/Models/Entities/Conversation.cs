using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace SignalR.SelfHosted.Messages.Models.Entities;

public class Conversation
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int CreatorUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
