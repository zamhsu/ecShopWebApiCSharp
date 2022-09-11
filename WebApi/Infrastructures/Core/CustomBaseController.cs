using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace WebApi.Infrastructures.Core
{
    public class CustomBaseController : ControllerBase
    {
        private string _userJwt;
        private string _userGuid;
        private string _userRole;

        /// <summary>
        /// 使用者JWT
        /// </summary>
        protected string UserJwt 
        {
            get => GetJwt();
            set => _userJwt = value;
        }

        /// <summary>
        /// 使用者Guid
        /// </summary>
        protected string UserGuid 
        { 
            get => GetUserGuidFromJwt(UserJwt); 
            set => _userGuid = value; 
        }

        /// <summary>
        /// 使用者角色
        /// </summary>
        protected string UserRole 
        { 
            get => GetUserRoleFromJwt(UserJwt); 
            set => _userRole = value; 
        }

        /// <summary>
        /// 取得Jwt
        /// </summary>
        /// <returns></returns>
        private string GetJwt()
        {
            string jwt = Request.Headers[HeaderNames.Authorization];
            return jwt;
        }

        /// <summary>
        /// 從JWT中取得Guid
        /// </summary>
        /// <param name="jwt">JWT</param>
        /// <returns></returns>
        private string GetUserGuidFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
            {
                return null;
            }

            Claim guidClaim = User.Claims.FirstOrDefault(q => q.Type == CustomClaimType.Guid);
            if (guidClaim == null)
            {
                return null;
            }

            string guid = guidClaim.Value;
            return guid;
        }

        /// <summary>
        /// 從JWT中取得使用者角色
        /// </summary>
        /// <param name="jwt">JWT</param>
        /// <returns></returns>
        private string GetUserRoleFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
            {
                return null;
            }

            Claim roleClaim = User.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                return null;
            }

            string role = roleClaim.Value;
            return role;
        }
    }
}