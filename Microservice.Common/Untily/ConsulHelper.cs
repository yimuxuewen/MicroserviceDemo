using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Common
{
    public static class ConsulHelper
    {
        public static bool ConsulRegist(this IConfiguration configuration)
        {
            try
            {
                //创建Consul客户端
                ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri(configuration["ConsulAddress"]);
                    c.Datacenter = configuration["ConsulCenter"];
                });
                //重命令行中获取 ip port weight(权重) 目的是重复反向代理
                string ip = configuration["ip"];
                int port = int.Parse(configuration["port"]);
                //当为空时权重为 10
                int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);
                string serviceid = $"{ip}-{port}-{weight}";
                //找到Consul中是否已经注册过服务，集群时会出现同一个客户端多次注册现象
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(10000);
                var nodes = client.Catalog.Nodes(cancellationTokenSource.Token).Result.Response;
                foreach (var item in nodes)
                {
                    int exitcount = client.Catalog.Node(item.Name).Result.Response.Services.Where(m => m.Key == serviceid && m.Value.Service == configuration["ConsulGroupName"]).Count();
                    if (exitcount > 0)
                    {
                        return false;
                    }
                }
                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = serviceid,//唯一的
                    Name = configuration["ConsulGroupName"],//服务组名称
                    Address = ip,//ip需要改动
                    Port = port,//不同实例
                    Tags = new string[] { weight.ToString() }, //权重设置
                    Check = new AgentServiceCheck() //健康检测
                    {
                        Interval = TimeSpan.FromSeconds(10), //每隔多久检测一次
                        HTTP = $"http://{ip}:{port}/api/Health/Index", //检测地址
                        Timeout = TimeSpan.FromSeconds(5), //多少秒为超时
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60) //在遇到异常后关闭自身服务通道,最短60s，小于60s不可移除
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //Console.WriteLine($"{ip}:{port} weight:{weight}");

        }
    }
}
