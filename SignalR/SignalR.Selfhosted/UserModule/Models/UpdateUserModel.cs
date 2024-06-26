namespace SignalR.Api.UserModule.Models;

public class UpdateUserModel : CreateUserModel
{
    public int Id { get; set; }
    public bool Active { get; set; }
}
