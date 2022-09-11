using AutoMapper;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.Members;
using Service.Interfaces.Members;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Helpers;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

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
        public async Task<ActionResult<BaseResponse<string>>> Login(BaseRequest<AdminMemberLoginParameter> baseRequest)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            AdminMemberInfoDto adminMemberInfo = await _adminMemberAccountService.LoginAsync(baseRequest.Data.Account, baseRequest.Data.Password);
            
            if (adminMemberInfo == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "登入失敗";

                return baseResponse;
            }

            DateTime expireDateTime = adminMemberInfo.ExpirationDate.DateTime;
            string jwt = _jwtHelper.GenerateToken(adminMemberInfo.UserName, adminMemberInfo.Guid, MemberRoleEnum.AdminMember, expireDateTime);

            baseResponse.IsSuccess = true;
            baseResponse.Data = jwt;

            return baseResponse;
        }

        [Authorize]
        [HttpGet("api/admin/login/check")]
        public async Task<ActionResult<BaseResponse<string>>> LoginCheck()
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            AdminMemberInfoDto adminMemberInfo = await _adminMemberAccountService.LoginByOnlyGuidAsync(UserGuid);
            
            if (adminMemberInfo == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "檢查失敗";

                return baseResponse;
            }

            DateTime expireDateTime = adminMemberInfo.ExpirationDate.DateTime;
            string jwt = _jwtHelper.GenerateToken(adminMemberInfo.UserName, adminMemberInfo.Guid, MemberRoleEnum.AdminMember, expireDateTime);

            baseResponse.IsSuccess = true;
            baseResponse.Data = jwt;

            return baseResponse;
        }
    }
}