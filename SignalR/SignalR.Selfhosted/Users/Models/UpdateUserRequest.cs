﻿namespace SignalR.SelfHosted.Users.Models;

public class UpdateUserRequest : CreateUserRequest
{
    public int Id { get; set; }
}