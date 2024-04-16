using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SignalR.Common.Constants;
using SignalR.SelfHosted.Notification;
using SignalR.SelfHosted.Users.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

    public TokenModel CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Id = _context.Users.Count + 1,
            FullName = request.FullName,
            PhotoUrl = request.PhotoUrl
        };
        _context.Users.Add(user);

        return PrepareToken(user);
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

    public TokenModel GenerateUserToken(CreateUserTokenRequest request)
    {
        var user = _context.Users.Where(x => x.FullName == request.FullName).FirstOrDefault();
        if (user == null)
        {
            return null;
        }

        return PrepareToken(user);
    }

    private static TokenModel PrepareToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstants.TOKEN_SECRET_KEY));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: AuthenticationConstants.ISSUER,
            audience: AuthenticationConstants.AUDIENCE,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            },
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        return new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            FullName = user.FullName,
        };
    }
}
