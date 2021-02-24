using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuthenticationCenter.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuthenticationCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {


        private readonly ILogger<AuthenticationController> _logger;
        private readonly IJWTService _jWTService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ILogger<AuthenticationController> logger,IJWTService jWTService, IConfiguration configuration)
        {
            _logger = logger;
            _jWTService = jWTService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]
        public IEnumerable<int> Get()
        {
            return new List<int>() { 1, 2, 34, 5, 7, 8 };
        }

        [HttpGet]
        [Route("Login")]
        public string Login(string name,string password)
        {
            if ("Yang".Equals(name) && "123456".Equals(password))
            {
                string token = _jWTService.GetToken(name);
                return JsonConvert.SerializeObject(new { result = true, token });
            }
            else
            {
                return JsonConvert.SerializeObject(new { result = false, token="" });
            }
        }
    }
}
