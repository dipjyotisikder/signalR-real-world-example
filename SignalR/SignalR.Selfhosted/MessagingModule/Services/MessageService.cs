using System;
using System.Linq;
using System.Threading.Tasks;
using SignalR.Api.Hubs.Services;
using SignalR.Api.MessagingModule.Models;
using SignalR.Api.UserModule.Services;
using SignalR.Api.MessagingModule.Models.Entities;
using SignalR.Api.UserModule.Models;
using SignalR.Api.Constants;

namespace SignalR.Api.MessagingModule.Services;

public class MessageService : IMessageService
{
    private readonly IDataContext _context;
    private readonly IHubService _hubService;
    private readonly IConversationService _conversationService;
    private readonly ICurrentUser _currentUser;

    public MessageService(
        IDataContext context,
        IHubService hubService,
        IConversationService conversationService,
        ICurrentUser currentUser)
    {
        _context = context;
        _hubService = hubService;
        _conversationService = conversationService;
        _currentUser = currentUser;
    }

    public async Task<MessageModel> Create(CreateMessageModel request)
    {
        // CREATE MESSAGE
        var message = new Message
        {
            Id = _context.Messages.Count() + 1,
            CreatedAt = DateTime.UtcNow,
            Text = request.Text,
            ConversationId = request.ConversationId,
            CreatorUserId = _currentUser.Id,
        };
        _context.Messages.Add(message);

        // ADD USERS FROM CURRENT CONVERSATION
        var messageAudiences = (await _conversationService
            .GetAudiences(request.ConversationId))
            .AudienceUsers
            .Select(x => new MessageAudience
            {
                MessageId = message.Id,
                AudienceUserId = x.Id
            });

        _context.MessageAudiences.AddRange(messageAudiences);
        await _context.SaveChangesAsync();

        var messageModel = GetMessageModel(message.Id);

        await _hubService
            .SendToGroupsAsync(
                groups: messageAudiences.Select(x => x.AudienceUserId.ToString()),
                eventName: HubEventName.Create(HubConstants.Events.MESSAGE_IS_CREATED),
                payload: messageModel);

        return messageModel;
    }

    private MessageModel GetMessageModel(int messageId)
    {
        var messageModel = (from message in _context.Messages
                            where message.Id == messageId
                            join user in _context.Users on message.CreatorUserId equals user.Id into creatorUsers
                            from user in creatorUsers.DefaultIfEmpty()
                            select new MessageModel
                            {
                                Id = message.Id,
                                Text = message.Text,
                                CreatedAt = message.CreatedAt,
                                CreatorUser = user == null ? null : new UserModel
                                {
                                    Id = user.Id,
                                    FullName = user.FullName,
                                    PhotoUrl = user.PhotoUrl,
                                }

                            }).FirstOrDefault();

        return messageModel;
    }
}
