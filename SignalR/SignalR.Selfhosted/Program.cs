using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using SignalR.Api.MessagingModule.Services;
using SignalR.Api.UserModule.Services;
using SignalR.Api.Hubs.Services;
using SignalR.Api.Data.SqLite;
using SignalR.Api.Hubs;
using SignalR.Api.Constants;
using SignalR.Api.Infrastructure.DependencyInjection;

// SERVICE CONTAINER
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AuthenticationConstants.ISSUER,
            ValidAudience = AuthenticationConstants.AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstants.TOKEN_SECRET_KEY)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services
    .AddSqLiteDatabase(builder.Configuration);
// .AddInMemoryDatabase(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddInfrastructureDependencies();

// PIPELINE
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

app.UseMiddleware<HubMiddleWare>();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.MapControllers();

app.MapHub<ApplicationHub>(HubConstants.HUB_ENDPOINT);

app.Run();
