using Microsoft.AspNetCore.Http.Connections;
using System.Threading.Tasks;

namespace SignalR.Api.Infrastructure.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAzureSignalRService
    {
        /// <summary>
        /// Adds a client connection into a group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>An object of <see cref="NegotiationResponse"/>.</returns>
        Task<NegotiationResponse> AddToGroupAsync(string groupName);

        /// <summary>
        /// Send to all of the clients connected to a certain group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns>A completed task.</returns>
        Task SendToGroupAsync<T>(string groupName, T message);
    }
}
