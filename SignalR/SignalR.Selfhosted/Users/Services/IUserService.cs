using SignalR.SelfHosted.Users.Models;
using SignalR.SelfHosted.Users.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Users.Services;

public interface IUserService
{
    IEnumerable<User> GetUsers();

    Task<TokenModel> CreateUser(CreateUserModel request);

    Task<User> UpdateUser(UpdateUserModel request);

    TokenModel RefreshUserToken(RefreshUserTokenModel request);

    Task OnLineUser(bool onLine, int userId);

    Task TriggerUserIsTypingEvent(int conversationId, bool isTyping);
}
