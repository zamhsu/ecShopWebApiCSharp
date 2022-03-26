using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Members;
using WebApi.Core;
using WebApi.Dtos;
using WebApi.Dtos.Members;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Members;

namespace WebApi.Controllers
{
    [ApiController]
    public class AdminMemberAccountController : CustomBaseController
    {
        private readonly IAdminMemberService _adminMemberService;
        private readonly IAdminMemberAccountService _adminMemberAccountService;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public AdminMemberAccountController(IAdminMemberService adminMemberService,
            IAdminMemberAccountService adminMemberAccountService,
            IMapper mapper,
            JwtHelper jwtHelper)
        {
            _adminMemberService = adminMemberService;
            _adminMemberAccountService = adminMemberAccountService;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("api/admin/login")]
        public async Task<ActionResult<BaseResponse<string>>> Login(BaseRequest<AdminMemberLoginModel> baseRequest)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            AdminMemberInfoModel? adminMemberInfo = await _adminMemberAccountService.LoginAsync(baseRequest.Data.Account, baseRequest.Data.Password);
            
            if (adminMemberInfo == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "登入失敗";

                return baseResponse;
            }

            DateTime expireDateTime = adminMemberInfo.ExpirationDate.DateTime;
            string jwt = _jwtHelper.GenerateToken(adminMemberInfo.UserName, adminMemberInfo.Guid, MemberRolePara.AdminMember, expireDateTime);

            baseResponse.IsSuccess = true;
            baseResponse.Data = jwt;

            return baseResponse;
        }

        [Authorize]
        [HttpGet("api/admin/login/check")]
        public async Task<ActionResult<BaseResponse<string>>> LoginCheck()
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            AdminMemberInfoModel? adminMemberInfo = await _adminMemberAccountService.LoginByOnlyGuidAsync(UserGuid);
            
            if (adminMemberInfo == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "檢查失敗";

                return baseResponse;
            }

            DateTime expireDateTime = adminMemberInfo.ExpirationDate.DateTime;
            string jwt = _jwtHelper.GenerateToken(adminMemberInfo.UserName, adminMemberInfo.Guid, MemberRolePara.AdminMember, expireDateTime);

            baseResponse.IsSuccess = true;
            baseResponse.Data = jwt;

            return baseResponse;
        }
    }
}