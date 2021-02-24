using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCenter.Utility
{
    public interface IJWTService
    {
        string GetToken(string UserName);
    }
    public class JWTService : IJWTService
    {
        private IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetToken(string UserName)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name,UserName),
                new Claim("NickName","Ric"),
                new Claim("Role","Administrator")
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /*
               Claims(预载信息)
               Claims部分包含一些跟token有关的重要信息

               issuer 签发人
               audience 主题
               expires 过期时间 ，Unix时间格式
               iat 创建时间 ，Unix时间格式
               jti 针对单前token的唯一标识
             */

            var token = new JwtSecurityToken(
                issuer: _configuration["issuer"],
                audience: _configuration["audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds
                );
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
