namespace SignalR.Api.UserModule.Models;

public class UserModel
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string PhotoUrl { get; set; }

    public bool IsTyping { get; set; } = false;
}
