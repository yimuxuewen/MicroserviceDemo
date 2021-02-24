using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IConfiguration _configuration;

        public AuthenticationController(ILogger<AuthenticationController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            return new JsonResult(new { Data = "这个OK的，不需要鉴权授权" });
        }

        [HttpGet]
        [Route("GetAuthorizeData")]
        [Authorize]
        public IActionResult GetAuthorizeData()
        {
            var Name = base.HttpContext.AuthenticateAsync().Result.Principal.Claims.FirstOrDefault(a => a.Type.Equals("NickName"))?.Value;
            return new JsonResult(
                new
                {
                    Data = $"已授权,用户名：{Name}",
                    Type = "GetAuthorizeData"
                });
        }
    }
}
