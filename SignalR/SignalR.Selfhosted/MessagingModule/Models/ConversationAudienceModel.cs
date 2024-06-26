using SignalR.Api.UserModule.Models;
using System.Collections.Generic;

namespace SignalR.Api.MessagingModule.Models;

public class ConversationAudienceModel : ConversationModel
{
    public IEnumerable<UserModel> AudienceUsers { get; set; }
}
