using System;
using qwizd_api.Data.Model;

namespace qwizd_api.Service.Contract;

public interface IUserService
{
    User? GetUser(string userEmail);

    User CreateUser(string userEmail);
}
