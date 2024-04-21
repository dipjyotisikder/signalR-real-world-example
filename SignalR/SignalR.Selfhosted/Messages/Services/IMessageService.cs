using SignalR.SelfHosted.Messages.Models;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Messages.Services;

public interface IMessageService
{
    Task<MessageModel> Create(CreateMessageModel request);
}
