using SignalR.SelfHosted.Users.Models;
using System;

namespace SignalR.SelfHosted.Messages.Models;

public class MessageModel
{
    public int Id { get; set; }

    public string Text { get; set; }

    public UserModel CreatorUser { get; set; }

    public DateTime CreatedAt { get; set; }
}
