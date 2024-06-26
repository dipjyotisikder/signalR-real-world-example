using Microsoft.IdentityModel.Tokens;
using SignalR.Api.Constants;
using SignalR.Api.Hubs.Services;
using SignalR.Api.UserModule.Models;
using SignalR.Api.UserModule.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Api.UserModule.Services;

public class UserService : IUserService
{
    private readonly IDataContext _context;
    private readonly IHubService _hubService;
    private readonly ICurrentUser _currentUser;

    public UserService(IDataContext context, IHubService hubService, ICurrentUser currentUser)
    {
        _context = context;
        _hubService = hubService;
        _currentUser = currentUser;
    }

    public async Task<TokenModel> CreateUser(CreateUserModel request)
    {
        var user = _context.Users.FirstOrDefault(x => x.FullNameNormalized == request.FullName.ToUpper());
        if (user == null)
        {
            user = new User(id: _context.Users.Count() + 1, fullName: request.FullName, photoUrl: request.PhotoUrl);
            _context.Users.Add(user);
        }

        var token = PrepareTokenEntity(user);
        _context.Tokens.Add(token);

        await _context.SaveChangesAsync();

        return PrepareToken(user, token.RefreshToken);
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users;
    }

    public async Task<User> UpdateUser(UpdateUserModel request)
    {
        var user = _context.Users.Where(x => x.Id == request.Id).FirstOrDefault();
        if (user == null)
        {
            return null;
        }

        user.SetFullName(request.FullName);
        user.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync();

        return user;
    }

    public TokenModel RefreshUserToken(RefreshUserTokenModel request)
    {
        var existedToken = _context.Tokens.FirstOrDefault(x => x.RefreshToken == request.RefreshToken);
        if (existedToken == null)
        {
            return null;
        }

        var user = _context.Users.FirstOrDefault(x => x.Id == existedToken.TokenUserId);
        if (user == null)
        {
            return null;
        }

        if (existedToken.IsExpired)
        {
            return null;
        }

        return PrepareToken(user, request.RefreshToken);
    }

    private static TokenModel PrepareToken(User user, string refreshToken)
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
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        return new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    private static Token PrepareTokenEntity(User user)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var token = new Token(user.Id, refreshToken);
        return token;
    }

    public Task TriggerUserIsTypingEvent(int conversationId, bool isTyping)
    {
        var audienceUserIds = _context.ConversationAudiences
            .Where(x => x.ConversationId == conversationId && x.AudienceUserId != _currentUser.Id)
            .Select(x => x.AudienceUserId.ToString()).Distinct();

        var typingUser = _context.Users
           .Where(x => x.Id == _currentUser.Id)
           .Select(x => new UserModel
           {
               Id = x.Id,
               FullName = x.FullName,
               PhotoUrl = x.PhotoUrl,
               IsTyping = isTyping
           }).FirstOrDefault();

        return _hubService.SendToGroupsAsync(
                    groups: audienceUserIds,
                    eventName: HubEventName.Create(HubConstants.Events.USER_IS_TYPING),
                    typingUser);
    }
}
