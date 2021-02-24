using Microservice.Model;
using System;
using System.Collections.Generic;

namespace Microservice.Interface
{
    public interface IUserService
    {
        public User FindUser(int id);

        public IEnumerable<User> UserAll();
    }
}
