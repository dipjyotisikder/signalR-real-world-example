namespace SignalR.SelfHosted.Users.Models;

public class UserModel
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string PhotoUrl { get; set; }

    public bool OnLine { get; set; }
}
