using SignalR.Api.MessagingModule.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.MessagingModule.Services;

public interface IConversationService
{
    IEnumerable<ConversationModel> GetAll();

    IEnumerable<MessageModel> GetMessages(int conversationId);

    Task<ConversationAudienceModel> GetAudiences(int conversationId);

    Task<ConversationModel> Create(CreateConversationModel request);
}
