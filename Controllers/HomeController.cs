using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroServiceDemo.Models;
using MicroService.IService;
using MicroService;
using MicroService.Model;
using Consul;

namespace MicroServiceDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        static int _iseed = 0;
        private static int iSeed
        {
            get
            {
                return _iseed;
            }
            set
            {
                if (value > 999999)
                {
                    _iseed = 0;
                }
                else
                {
                    _iseed = value;
                }
            }
        }

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            //string url = "http://localhost:5002/api/Users/ALL";
            //string content = WebApiHelper.InvokeApi(url);
            //ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            //ViewBag.Users = _userService.UserAll();

            string url = "http://UserInfoService/api/Users/ALL";
            Uri uri = new Uri(url);
            string groupName = uri.Host;
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500/");
                c.Datacenter = "dc1";
            });
            var response = client.Agent.Services().Result.Response;
            ///找到服务名为groupName的所有服务
            var serviceDict = response.Values.Where(m => m.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();
            #region 负载均衡
            AgentService agentService = null;
            //平均分配法
            {
                //agentService = serviceDict[new Random(iSeed++).Next(0, serviceDict.Length)];
            }
            //轮询分配法
            {
                //agentService = serviceDict[iSeed++ % serviceDict.Length];
            }
            //权重分配法
            {
                List<AgentService> pairsList = new List<AgentService>();
                foreach (var pair in serviceDict)
                {
                    int count = int.Parse(pair.Tags?[0]);
                    for (int i = 0; i < count; i++)
                    {
                        pairsList.Add(pair);
                    }
                }
                agentService = pairsList[new Random(iSeed++).Next(0, pairsList.Count)];
            }
            #endregion

            string baseurl = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            Console.WriteLine(baseurl);
            string content = WebApiHelper.InvokeApi(baseurl);
            ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            ViewBag.Users = _userService.UserAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
