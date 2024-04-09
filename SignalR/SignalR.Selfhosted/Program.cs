using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Common.Constants;
using SignalR.SelfHosted.Notification;
using SignalR.SelfHosted.Notification.Services;
using SignalR.SelfHosted.Users.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSignalR();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(builder => builder
    .SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseRouting();

app.UseWebSockets();
app.MapControllers();

app.MapHub<NotificationHub>(Constants.HubEndpoint);

app.Run();
