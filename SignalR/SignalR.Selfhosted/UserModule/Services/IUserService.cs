using SignalR.Api.UserModule.Models;
using SignalR.Api.UserModule.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.UserModule.Services;

public interface IUserService
{
    IEnumerable<User> GetUsers();

    Task<TokenModel> CreateUser(CreateUserModel request);

    Task<User> UpdateUser(UpdateUserModel request);

    TokenModel RefreshUserToken(RefreshUserTokenModel request);

    Task TriggerUserIsTypingEvent(int conversationId, bool isTyping);
}
