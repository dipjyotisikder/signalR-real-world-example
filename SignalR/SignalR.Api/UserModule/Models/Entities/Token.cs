using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace SignalR.Api.UserModule.Models.Entities;

public class Token
{
    public Token(int tokenUserId, string refreshToken)
    {
        ExpireAt = DateTime.UtcNow.AddDays(10);
        TokenUserId = tokenUserId;
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; set; }

    public int TokenUserId { get; set; }

    public DateTime ExpireAt { get; private set; }

    public bool IsExpired => ExpireAt <= DateTime.UtcNow;
}

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.RefreshToken);
        builder.Property(x => x.RefreshToken).ValueGeneratedNever();
    }
}