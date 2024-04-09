using Microsoft.AspNetCore.Mvc;
using SignalR.SelfHosted.Users.Models;
using SignalR.SelfHosted.Users.Services;

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
    public IActionResult CreateUsers([FromBody] CreateUserRequest request)
    {
        return Ok(_userService.CreateUser(request));
    }

    [HttpPut]
    public IActionResult UpdateUsers([FromBody] UpdateUserRequest request)
    {
        return Ok(_userService.UpdateUser(request));
    }
}