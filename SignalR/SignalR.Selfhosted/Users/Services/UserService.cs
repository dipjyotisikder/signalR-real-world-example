using SignalR.SelfHosted.Users.Models;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.SelfHosted.Users.Services;

public class UserService : IUserService
{
    public User CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Id = UserProvider.Users.Count + 1,
            FullName = request.FullName,
            PhotoUrl = request.PhotoUrl
        };
        UserProvider.Users.Add(user);

        return user;
    }

    public IEnumerable<User> GetUsers()
    {
        return UserProvider.Users;
    }

    public User UpdateUser(UpdateUserRequest request)
    {
        var user = UserProvider.Users.Where(x => x.Id == request.Id).FirstOrDefault();
        if (user == null)
        {
            return null;
        }

        user.FullName = request.FullName;
        user.PhotoUrl = request.PhotoUrl;

        return user;
    }
}
