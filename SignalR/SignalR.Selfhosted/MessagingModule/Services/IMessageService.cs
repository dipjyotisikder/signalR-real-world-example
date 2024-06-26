using SignalR.Api.MessagingModule.Models;
using System.Threading.Tasks;

namespace SignalR.Api.MessagingModule.Services;

public interface IMessageService
{
    Task<MessageModel> Create(CreateMessageModel request);
}
