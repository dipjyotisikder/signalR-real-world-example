using SignalR.Api.UserModule.Models;
using System;

namespace SignalR.Api.MessagingModule.Models;

public class ConversationModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public UserModel CreatorUser { get; set; }

    public DateTime CreatedAt { get; set; }
}
