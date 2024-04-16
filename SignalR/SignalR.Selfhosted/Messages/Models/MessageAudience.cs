namespace SignalR.SelfHosted.Messages.Models;

public class MessageAudience
{
    public int MessageId { get; set; }

    public int AudienceUserId { get; set; }

    public bool Seen { get; set; } = false;
}
