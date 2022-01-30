using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 產生JWT
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <param name="guid">使用者帳號GUID</param>
        /// <param name="memberRole">角色</param>
        /// <param name="expireDateTime">失效日期</param>
        /// <returns></returns>
        public string GenerateToken(string userName, string guid, MemberRolePara memberRole, DateTime expireDateTime)
        {
            string issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            string signKey = _configuration.GetValue<string>("JwtSettings:SignKey");

            // JWT 的聲明資訊
            List<Claim> claims = new List<Claim>();

            // claims 加入userName
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            // claims 加入jwt 的唯一識別碼
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            // 自行擴充claims
            claims.Add(new Claim(CustomClaimTypes.Guid, guid));
            claims.Add(new Claim("role", memberRole.ToString("D")));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            // 使用HmacSha256建立簽章
            SigningCredentials signingCredentials = HashUtility.CreateHmacSha256Signature(signKey);

            // 建立 SecurityTokenDescriptor
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Subject = claimsIdentity,
                Expires = expireDateTime,
                SigningCredentials = signingCredentials
            };

            // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
    }
}