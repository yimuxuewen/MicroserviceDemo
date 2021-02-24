using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.AuthenticationCenter
{
    /// <summary>
    /// 自定义的管理信息
    /// </summary>
    public class InitConfig
    {
        /// <summary>
        /// 定义ApiResource
        /// </summary>
        /// <returns>返回多个ApiResource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi","用户获取API")

            };
        }

        /// 定义ApiResource
        /// </summary>
        /// <returns>返回多个ApiResource</returns>
        public static IEnumerable<Client> GetClients()
        {
            List<string> scopes = new List<string>();
            scopes.Add("UserApi");
            scopes.Add(IdentityServerConstants.StandardScopes.OpenId);
            scopes.Add(IdentityServerConstants.StandardScopes.Profile);

            return new[]
            {
                new Client
                {
                    ClientId="AuthenticationCenter",//客户端唯一标识
                    ClientSecrets=new []{new Secret("YANG".Sha256()) },//客户端密码，进行了加密
                    AllowedGrantTypes=GrantTypes.ClientCredentials,//授权方式，客户端认证，只要ClientId+ClientSecrets
                    AllowedScopes={ "UserApi"},//运行访问的资源

                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
    => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };
    }
}
