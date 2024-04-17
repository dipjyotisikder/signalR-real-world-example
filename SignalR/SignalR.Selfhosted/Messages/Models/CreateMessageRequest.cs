namespace SignalR.SelfHosted.Messages.Models;

public class CreateMessageRequest
{
    public int ConversationId { get; set; }

    public string Text { get; set; }
}
