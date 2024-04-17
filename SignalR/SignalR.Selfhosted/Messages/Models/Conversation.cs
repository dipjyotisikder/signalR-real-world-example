using System;

namespace SignalR.SelfHosted.Messages.Models;

public class Conversation
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int CreatorUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
