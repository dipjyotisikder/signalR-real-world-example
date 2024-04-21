using Microsoft.AspNetCore.Mvc;
using SignalR.SelfHosted.Users.Models;
using SignalR.SelfHosted.Users.Services;
using System.Threading.Tasks;

namespace SignalR.SelfHosted.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_userService.GetUsers());
    }

    [HttpPost]
    public async Task<IActionResult> CreateUsers([FromBody] CreateUserModel request)
    {
        return Ok(await _userService.CreateUser(request));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUsers([FromBody] UpdateUserModel request)
    {
        return Ok(await _userService.UpdateUser(request));
    }

    [HttpPost("refreshToken")]
    public IActionResult RefreshToken([FromBody] RefreshUserTokenModel request)
    {
        return Ok(_userService.RefreshUserToken(request));
    }
}