using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microservice.ServiceInstance
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IConfiguration _configuration;

        public HealthController(IConfiguration configuration,ILogger<HealthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        // GET: api/<controller>
        [HttpGet("Index")]
        public IActionResult Get()
        {
            Console.WriteLine($"This is HealthController {_configuration["port"]} Invoke");
            _logger.LogDebug($"This is HealthController {_configuration["port"]} Invoke");
            return Ok();
        }
    }
}
