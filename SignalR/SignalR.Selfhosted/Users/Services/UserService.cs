using Microsoft.AspNetCore.SignalR;
using SignalR.SelfHosted.Notification;
using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Users.Services;

public class UserService : IUserService
{
    private readonly IDataContext _context;

    private readonly IHubContext<ApplicationHub> _hubContext;

    public UserService(IHubContext<ApplicationHub> hubContext, IDataContext context)
    {
        _hubContext = hubContext;
        _context = context;
    }

    public User CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Id = _context.Users.Count + 1,
            FullName = request.FullName,
            PhotoUrl = request.PhotoUrl
        };
        _context.Users.Add(user);

        return user;
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users;
    }

    public async Task OnLineUser(bool onLine, int userId)
    {
        var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
        if (user is not null)
        {
            user.OnLine = onLine;
        }

        var conversationIds = _context.ConversationAudiences
            .Where(x => x.AudienceUserId == userId)
            .Select(x => x.ConversationId).Distinct();

        var audienceUserIds = _context.ConversationAudiences
                    .Where(x => conversationIds.Contains(x.ConversationId))
                    .Select(x => x.AudienceUserId).Distinct();

        foreach (var audienceUserId in audienceUserIds)
        {
            await _hubContext.Clients.Group(audienceUserId.ToString()).SendAsync("UserOnLine", userId);
        }
    }

    public User UpdateUser(UpdateUserRequest request)
    {
        var user = _context.Users.Where(x => x.Id == request.Id).FirstOrDefault();
        if (user == null)
        {
            return null;
        }

        user.FullName = request.FullName;
        user.PhotoUrl = request.PhotoUrl;
        user.OnLine = request.Active;

        return user;
    }
}
