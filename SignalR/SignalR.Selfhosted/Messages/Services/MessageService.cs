using SignalR.SelfHosted.Messages.Models;
using SignalR.SelfHosted.Notification.Services;
using SignalR.SelfHosted.Notifications.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Messages.Services;

public class MessageService : IMessageService
{
    private readonly IDataContext _context;
    private readonly IHubService _hubService;
    private readonly IConversationService _conversationService;

    public MessageService(
        IDataContext context,
        IHubService hubService,
        IConversationService conversationService)
    {
        _context = context;
        _hubService = hubService;
        _conversationService = conversationService;
    }

    public async Task<MessageModel> Create(CreateMessageRequest request)
    {
        // CREATE MESSAGE
        var message = new Message
        {
            Id = _context.Messages.Count + 1,
            CreatedAt = DateTime.UtcNow,
            Text = request.Text,
            ConversationId = request.ConversationId,
        };
        _context.Messages.Add(message);

        // ADD USERS FROM CURRENT CONVERSATION
        var messageAudiences = _conversationService
            .GetAudiences(request.ConversationId)
            .Select(x => new MessageAudience
            {
                MessageId = message.Id,
                AudienceUserId = x.AudienceUser.Id,
                Seen = false
            });
        _context.MessageAudiences.AddRange(messageAudiences);

        var messageModel = GetMessageModel(message.Id);

        await _hubService
            .SendToGroupsAsync(
                groups: messageAudiences.Select(x => x.AudienceUserId.ToString()).ToArray(),
                eventName: HubEventName.Create("MessageCreated"),
                payload: messageModel);

        return messageModel;
    }

    private MessageModel GetMessageModel(int messageId)
    {
        var messageModel = (from message in _context.Messages
                            where message.Id == messageId
                            join conversation in _context.Conversations on message.ConversationId equals conversation.Id
                            join user in _context.Users on conversation.CreatorUserId equals user.Id into creatorUsers
                            from user in creatorUsers.DefaultIfEmpty()

                            select new MessageModel
                            {
                                Id = message.Id,
                                Text = message.Text,
                                CreatedAt = message.CreatedAt,
                                Conversation = new ConversationModel
                                {
                                    Id = conversation.Id,
                                    Title = conversation.Title,
                                    CreatedAt = conversation.CreatedAt,
                                    CreatorUser = user == null ? null : new Users.Models.UserModel
                                    {
                                        Id = user.Id,
                                        FullName = user.FullName,
                                        PhotoUrl = user.PhotoUrl,
                                    }
                                }
                            }).FirstOrDefault();

        return messageModel;
    }
}
