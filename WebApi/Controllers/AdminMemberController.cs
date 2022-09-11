using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.Members;
using Service.Interfaces.Members;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Models.Dtos.Members;
using WebApi.Infrastructures.Models.Paramaters;
using WebApi.Infrastructures.Models.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class AdminMemberController : CustomBaseController
    {
        private readonly IAdminMemberService _adminMemberService;
        private readonly IAdminMemberAccountService _adminMemberAccountService;
        private readonly IMapper _mapper;

        public AdminMemberController(IAdminMemberService adminMemberService,
            IAdminMemberAccountService adminMemberAccountService,
            IMapper mapper)
        {
            _adminMemberService = adminMemberService;
            _adminMemberAccountService = adminMemberAccountService;
            _mapper = mapper;
        }

        [HttpGet("api/admin/adminMember")]
        public ActionResult<BaseResponse<AdminMemberGetAdminMemberViewModel>> GetAdminMember([FromQuery] PageParameter pageParameter)
        {
            BaseResponse<AdminMemberGetAdminMemberViewModel> baseResponse = new BaseResponse<AdminMemberGetAdminMemberViewModel>();

            PagedList<AdminMemberDetailDto> pagedList = _adminMemberService.GetPagedDetailAll(pageParameter.PageSize, pageParameter.Page);
            List<AdminMemberDisplayDto> adminMemberDisplays = _mapper.Map<List<AdminMemberDisplayDto>>(pagedList.PagedData);
            Pagination pagination = pagedList.Pagination;

            AdminMemberGetAdminMemberViewModel viewModel = new AdminMemberGetAdminMemberViewModel()
            {
                AdminMemberDisplays = adminMemberDisplays,
                Pagination = pagination
            };

            baseResponse.IsSuccess = true;
            baseResponse.Data = viewModel;

            return baseResponse;
        }

        [HttpGet("api/admin/adminMember/{guid}")]
        public async Task<ActionResult<BaseResponse<AdminMemberDisplayDto>>> GetAdminMember(string guid)
        {
            BaseResponse<AdminMemberDisplayDto> baseResponse = new BaseResponse<AdminMemberDisplayDto>();

            AdminMemberDetailDto adminMember = await _adminMemberService.GetDetailByGuidAsync(guid);
            AdminMemberDisplayDto adminMemberDisplay = _mapper.Map<AdminMemberDisplayDto>(adminMember);

            if (adminMember is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "沒有資料";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = adminMemberDisplay;

            return baseResponse;
        }

        [HttpPut("api/admin/adminMember/{guid}/userInfo")]
        public async Task<ActionResult<BaseResponse<bool>>> PutAdminMemberInfo(string guid, BaseRequest<UpdateAdminMemberInfoParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            AdminMemberDto existedAdminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (existedAdminMember is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            AdminMemberUserInfoDto userInfoDto = _mapper.Map<AdminMemberUserInfoDto>(baseRequest.Data);
            userInfoDto.Guid = guid;

            bool result = await _adminMemberService.UpdateUserInfoAsync(userInfoDto);

            if(result.Equals(true))
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "修改成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "修改失敗";
            }

            return baseResponse;
        }

        [HttpPost("api/admin/adminMember")]
        public async Task<ActionResult<BaseResponse<bool>>> PostAdminMember(BaseRequest<CreateAdminMemberParameter> baseRequest)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            AdminMemberRegisterDto registerDto = _mapper.Map<AdminMemberRegisterDto>(baseRequest.Data);

            bool result = await _adminMemberAccountService.RegisterAsync(registerDto);

            if(result.Equals(true))
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "建立成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "建立失敗";
            }

            return baseResponse;
        }

        [HttpDelete("api/admin/adminMember/{guid}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteAdminMember(string guid)
        {
            BaseResponse<bool> baseResponse = new BaseResponse<bool>();

            AdminMemberDto adminMemberDto = await _adminMemberService.GetByGuidAsync(guid);
            if (adminMemberDto is null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            bool result = await _adminMemberService.DeleteByGuidAsync(guid);

            if(result.Equals(true))
            {
                baseResponse.IsSuccess = true;
                baseResponse.Message = "刪除成功";
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "刪除失敗";
            }

            return baseResponse;
        }
    }
}