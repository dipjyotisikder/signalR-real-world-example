using System;

namespace SignalR.SelfHosted.Messages.Models;

public class Message
{
    public int Id { get; set; }

    public string Text { get; set; }

    public int ConversationId { get; set; }

    public int CreatorUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
