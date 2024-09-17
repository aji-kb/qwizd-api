using System;
using qwizd_api.Data;
using qwizd_api.Data.Model;
using qwizd_api.Service.Contract;

namespace qwizd_api.Service;

public class UserService : IUserService
{
    private readonly QwizdContext _qwizdContext;

    public UserService(QwizdContext qwizdContext)
    {
        _qwizdContext = qwizdContext;
    }

    public User? GetUser(string userEmail)
    {
        var user = _qwizdContext.User.FirstOrDefault(x=>x.Email == userEmail);
        return user;
    }

    public User CreateUser(string userEmail)
    {
        var user = new User
        {
            Email = userEmail
        };

        _qwizdContext.User.Add(user);
        _qwizdContext.SaveChanges();

        return user;
    }
}
