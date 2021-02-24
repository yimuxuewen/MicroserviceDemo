using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microservice.Interface;
using Microservice.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microservice.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, IUserService userService,IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            _logger.LogDebug($"This is UserControler {_configuration["port"]} Get Invoke");
            User user = _userService.FindUser(id);
            user.Role = $"{_configuration["ip"]}--{_configuration["port"]}";
            return user;
        }

        [HttpGet]

        [Route("All")]
        public  IEnumerable<User> Get()
        {
            Console.WriteLine($"This is UserControler {_configuration["port"]} Invoke");
            _logger.LogDebug($"This is UserControler {_configuration["port"]} Invoke");
            return _userService.UserAll().Select(u => new User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{_configuration["ip"]}--{_configuration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

        [HttpGet]
        [Route("TimeOut")]
        public IEnumerable<User> TimeOut()
        {

            Thread.Sleep(5000);
            Console.WriteLine($"This is UserControler {_configuration["port"]} Invoke TimeOut");
            return _userService.UserAll().Select(u => new User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{_configuration["ip"]}--{_configuration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

    }
}