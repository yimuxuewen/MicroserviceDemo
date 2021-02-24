using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microservice.ServiceInstanceAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IConfiguration _configuration;

        public TestController(ILogger<TestController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is TestController {_configuration["port"]} Invoke");

            return new JsonResult(
                new
                {
                    message = "This is TestContorllerIndex",
                    Port = _configuration["Port"],
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")
                });
        }

        [Authorize]
        [HttpGet]
        [Route("IndexA")]
        public IActionResult IndexA()
        {
            Console.WriteLine($"This is TestController {_configuration["port"]} Invoke");

            return new JsonResult(
                new
                {
                    message = "This is TestContorllerIndex",
                    Port = _configuration["Port"],
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")
                });
        }
    }
}