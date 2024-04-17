using System;

namespace SignalR.SelfHosted.Users.Models;

public class Token
{
    public string RefreshToken { get; set; }

    public int TokenUserId { get; set; }

    public DateTime ExpireAt { get; private set; }

    public bool IsExpired => ExpireAt <= DateTime.UtcNow;
    public void SetDefaultExpiryDate() => ExpireAt = DateTime.UtcNow.AddDays(10);
}
