using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Users.Services;

public interface IUserService
{
    IEnumerable<User> GetUsers();

    User CreateUser(CreateUserRequest request);

    User UpdateUser(UpdateUserRequest request);

    Task OnLineUser(bool onLine, int userId);
}
