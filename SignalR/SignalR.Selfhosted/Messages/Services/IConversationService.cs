using SignalR.SelfHosted.Messages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Messages.Services;

public interface IConversationService
{
    IEnumerable<ConversationModel> GetAll();

    IEnumerable<MessageModel> GetMessages(int conversationId);

    Task<ConversationAudienceModel> GetAudiences(int conversationId);

    Task<ConversationModel> Create(CreateConversationModel request);
}
