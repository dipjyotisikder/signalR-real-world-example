using Microsoft.AspNetCore.Mvc;
using SignalR.Api.UserModule.Models;
using SignalR.Api.UserModule.Models.Entities;
using SignalR.Api.UserModule.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Api.UserModule;

/// <summary>
/// Represents a controller method to manage users.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor for <see cref="UsersController"/>.
    /// </summary>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Represents a endpoint method to produce user list.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), statusCode: 200)]
    public IActionResult GetUsers()
    {
        return Ok(_userService.GetUsers());
    }

    /// <summary>
    /// Represents a endpoint method to create a user and an initial token.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TokenModel), statusCode: 200)]
    public async Task<IActionResult> CreateUsers([FromBody] CreateUserModel request)
    {
        return Ok(await _userService.CreateUser(request));
    }

    /// <summary>
    /// Represents a endpoint method to update a user.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(User), statusCode: 200)]
    public async Task<IActionResult> UpdateUsers([FromBody] UpdateUserModel request)
    {
        return Ok(await _userService.UpdateUser(request));
    }

    /// <summary>
    /// Represents a endpoint method to refresh a user's token.
    /// </summary>
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(TokenModel), statusCode: 200)]
    public IActionResult RefreshToken([FromBody] RefreshUserTokenModel request)
    {
        return Ok(_userService.RefreshUserToken(request));
    }
}