using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Azure.SignalR.Management;
using Microsoft.AspNetCore.Http.Connections;
using SignalR.Common.Constants;
using SignalR.Common.Models;
using Azure.Core.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace SignalR.Api.Infrastructure.Services;

/// <summary>
/// Class to create Azure signalr operations.
/// </summary>
public class AzureSignalRService : IAzureSignalRService
{
    private readonly ServiceHubContext _serviceHubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constractor for <see cref="AzureSignalRService"/>.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="configuration"></param>
    public AzureSignalRService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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
    }

    /// <inheritdoc/>
    public async Task<NegotiationResponse> AddToGroupAsync(string groupName)
    {
        var userId = $"User.{Guid.NewGuid()}";
        var negotiationResponse = await _serviceHubContext
            .NegotiateAsync(new NegotiationOptions
            {
                HttpContext = _httpContextAccessor.HttpContext,
                UserId = userId
            });

        await _serviceHubContext.UserGroups.AddToGroupAsync(userId, groupName);

        return negotiationResponse;
    }

    /// <inheritdoc/>
    public async Task SendToGroupAsync(string groupName)
    {
        await _serviceHubContext.Clients
            .Group(groupName)
            .SendCoreAsync(AzureHubConstants.NotificationCreatedEvent,
                new object[]{
                        new NotificationMessageModel
                        {
                            Id = 1,
                            Content = "Some Content",
                            Title = "Some Title"
                        }
                });
    }
}
