using Azure.Core.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SignalR.Common.Constants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTER AZURE SIGNALR
builder.Services
    .AddSingleton(new ServiceManagerBuilder()
    .WithOptions(option =>
    {
        option.ConnectionString = builder.Configuration.GetConnectionString(AzureHubConstants.AzureSignalRConnectionKey);

        option.UseJsonObjectSerializer(new NewtonsoftJsonObjectSerializer(new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        }));
    })
    .BuildServiceManager()
    .CreateHubContextAsync(AzureHubConstants.HubName, default)
    .ConfigureAwait(false)
    .GetAwaiter()
    .GetResult());

// PIPELINE
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder => builder
    .SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthorization();

app.UseWebSockets();
app.MapControllers();

app.Run();
