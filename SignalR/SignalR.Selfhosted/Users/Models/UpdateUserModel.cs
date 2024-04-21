namespace SignalR.SelfHosted.Users.Models;

public class UpdateUserModel : CreateUserModel
{
    public int Id { get; set; }
    public bool Active { get; set; }
}
