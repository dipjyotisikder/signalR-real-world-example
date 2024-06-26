using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Azure.SignalR.Management;
using Microsoft.AspNetCore.Http.Connections;
using Azure.Core.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using SignalR.Api.UserModule.Services;
using SignalR.Api.Constants;

namespace SignalR.Api.Infrastructure.Services;

/// <summary>
/// Class to create Azure signalr operations.
/// </summary>
public class AzureSignalRService : IAzureSignalRService
{
    private readonly ServiceHubContext _serviceHubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Constractor for <see cref="AzureSignalRService"/>.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="configuration"></param>
    public AzureSignalRService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ICurrentUser currentUser)
    {
        _httpContextAccessor = httpContextAccessor;

        _serviceHubContext = new ServiceManagerBuilder()
            .WithOptions(option =>
            {
                option.ConnectionString = _configuration.GetConnectionString(AzureHubConstants.AzureSignalRConnectionKey);

                option.UseJsonObjectSerializer(
                    new NewtonsoftJsonObjectSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        }));
            })
            .BuildServiceManager()
            .CreateHubContextAsync(AzureHubConstants.HubName, default)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        _configuration = configuration;
        _currentUser = currentUser;
    }

    /// <inheritdoc/>
    public async Task<NegotiationResponse> AddToGroupAsync(string groupName)
    {
        var negotiationResponse = await _serviceHubContext
            .NegotiateAsync(new NegotiationOptions
            {
                HttpContext = _httpContextAccessor.HttpContext,
                UserId = _currentUser.Id.ToString(),
            });

        await _serviceHubContext.UserGroups.AddToGroupAsync(_currentUser.Id.ToString(), groupName);

        return negotiationResponse;
    }

    /// <inheritdoc/>
    public async Task SendToGroupAsync<T>(string groupName, T message)
    {
        await _serviceHubContext.Clients
            .Group(groupName)
            .SendCoreAsync(AzureHubConstants.NotificationCreatedEvent, new object[] { message });
    }
}
