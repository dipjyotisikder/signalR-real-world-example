using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;

namespace SignalR.SelfHosted.Messages.Models;

public class ConversationAudienceModel : ConversationModel
{
    public IEnumerable<UserModel> AudienceUsers { get; set; }
}
