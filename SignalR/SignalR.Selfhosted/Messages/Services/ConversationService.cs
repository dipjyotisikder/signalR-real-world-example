using SignalR.SelfHosted.Messages.Models;
using SignalR.SelfHosted.Users.Models;
using SignalR.SelfHosted.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.SelfHosted.Messages.Services;

public class ConversationService : IConversationService
{
    private readonly IDataContext _context;
    private readonly ICurrentUser _currentUser;

    public ConversationService(IDataContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public ConversationModel Create(CreateConversationRequest request)
    {
        var currentUserId = _currentUser.Id;

        var conversation = new Conversation
        {
            Id = _context.Conversations.Count + 1,
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            CreatorUserId = currentUserId,
        };

        _context.Conversations.Add(conversation);

        _context.ConversationAudiences.Add(new ConversationAudience
        {
            ConversationId = conversation.Id,
            AudienceUserId = currentUserId
        });

        var creatorUser = _context.Users
            .Where(x => x.Id == conversation.CreatorUserId)
            .Select(x => new UserModel
            {
                Id = x.Id,
                FullName = x.FullName,
                PhotoUrl = x.PhotoUrl,
                OnLine = x.OnLine
            }).FirstOrDefault();

        return new ConversationModel
        {
            Id = conversation.Id,
            CreatedAt = conversation.CreatedAt,
            Title = conversation.Title,
            CreatorUser = creatorUser
        };
    }

    public IEnumerable<ConversationModel> GetAll()
    {
        return from c in _context.Conversations
               join cu in _context.Users
               on c.CreatorUserId equals cu.Id into creatorUsers
               from cu in creatorUsers.DefaultIfEmpty()
               select new ConversationModel
               {
                   Id = c.Id,
                   Title = c.Title,
                   CreatedAt = c.CreatedAt,
                   CreatorUser = cu == null ? null : new UserModel
                   {
                       Id = cu.Id,
                       FullName = cu.FullName,
                       OnLine = cu.OnLine,
                       PhotoUrl = cu.PhotoUrl
                   }
               };
    }

    public ConversationAudienceModel GetAudiences(int conversationId)
    {
        var conversation = _context.Conversations.FirstOrDefault(x => x.Id == conversationId);
        if (conversation == null)
        {
            return null;
        }

        // ADD CURRENT USER TO CONVERSATION
        var currentUserId = _currentUser.Id;
        var isAnAudience = _context.ConversationAudiences.Where(x => x.AudienceUserId == currentUserId && x.ConversationId == conversation.Id).Any();
        if (!isAnAudience)
        {
            _context.ConversationAudiences.Add(new ConversationAudience
            {
                AudienceUserId = currentUserId,
                ConversationId = conversation.Id
            });
        }

        var audiences = from ca in _context.ConversationAudiences.Where(x => x.ConversationId == conversationId)
                        join au in _context.Users on ca.AudienceUserId equals au.Id
                        select new UserModel
                        {
                            Id = au.Id,
                            OnLine = au.OnLine,
                            FullName = au.FullName,
                            PhotoUrl = au.PhotoUrl,
                        };

        var creatorUser = _context.Users
            .Where(x => x.Id == conversation.CreatorUserId)
            .Select(x => new UserModel
            {
                Id = x.Id,
                FullName = x.FullName,
                OnLine = x.OnLine,
                PhotoUrl = x.PhotoUrl,
            }).FirstOrDefault();

        return new ConversationAudienceModel
        {
            Id = conversation.Id,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            CreatorUser = creatorUser,
            AudienceUsers = audiences ?? Enumerable.Empty<UserModel>()
        };
    }

    public IEnumerable<MessageModel> GetMessages(int conversationId)
    {
        var messages = from m in _context.Messages.Where(x => x.ConversationId == conversationId)
                       join cu in _context.Users on m.CreatorUserId equals cu.Id into creatorUsers
                       from cu in creatorUsers.DefaultIfEmpty()
                       select new MessageModel
                       {
                           Id = m.Id,
                           Text = m.Text,
                           CreatedAt = m.CreatedAt,
                           CreatorUser = cu == null ? null : new UserModel
                           {
                               Id = cu.Id,
                               FullName = cu.FullName,
                               OnLine = cu.OnLine,
                               PhotoUrl = cu.PhotoUrl,
                           }
                       };
        return messages;
    }
}
