using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Users.Services;

public interface IUserService
{
    IEnumerable<User> GetUsers();

    TokenModel CreateUser(CreateUserRequest request);

    User UpdateUser(UpdateUserRequest request);

    TokenModel RefreshUserToken(RefreshUserTokenRequest request);

    Task OnLineUser(bool onLine, int userId);

    Task TriggerUserIsTypingEvent(int conversationId);
}
