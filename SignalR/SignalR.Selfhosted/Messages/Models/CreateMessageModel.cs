namespace SignalR.SelfHosted.Messages.Models;

public class CreateMessageModel
{
    public int ConversationId { get; set; }

    public string Text { get; set; }
}
