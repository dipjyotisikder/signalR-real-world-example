using Azure.Core.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SignalR.Serverless.Constants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<ServiceHubContext>(new ServiceManagerBuilder()
    .WithOptions(option =>
    {
        option.ConnectionString = builder.Configuration.GetConnectionString(CommonConstants.AzureSignalRConnectionKey);

        option.UseJsonObjectSerializer(new NewtonsoftJsonObjectSerializer(new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        }));
    })
    .BuildServiceManager()
    .CreateHubContextAsync(CommonConstants.NotificationHubName, default)
    .ConfigureAwait(false)
    .GetAwaiter()
    .GetResult());

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

app.MapControllers();

app.Run();
