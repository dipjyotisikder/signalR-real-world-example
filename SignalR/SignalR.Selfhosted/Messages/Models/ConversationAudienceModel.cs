using SignalR.SelfHosted.Users.Models;

namespace SignalR.SelfHosted.Messages.Models;

public class ConversationAudienceModel
{
    public ConversationModel Conversation { get; set; }

    public UserModel AudienceUser { get; set; }
}
