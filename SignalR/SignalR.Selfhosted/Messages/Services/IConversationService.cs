using SignalR.SelfHosted.Messages.Models;
using System.Collections.Generic;

namespace SignalR.SelfHosted.Messages.Services;

public interface IConversationService
{
    IEnumerable<ConversationModel> GetAll();

    IEnumerable<MessageModel> GetMessages(int conversationId);

    ConversationAudienceModel GetAudiences(int conversationId);

    ConversationModel Create(CreateConversationRequest request);
}
