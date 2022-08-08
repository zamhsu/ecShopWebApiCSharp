using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.Members;
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

            PagedList<AdminMember> pagedList = _adminMemberService.GetPagedDetailAll(pageParameter.PageSize, pageParameter.Page);
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

            AdminMember adminMember = await _adminMemberService.GetDetailByGuidAsync(guid);
            AdminMemberDisplayDto adminMemberDisplay = _mapper.Map<AdminMemberDisplayDto>(adminMember);

            if (adminMember == null)
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

            AdminMember existedAdminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (existedAdminMember == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            AdminMember adminMember = _mapper.Map<AdminMember>(baseRequest.Data);

            try
            {
                await _adminMemberService.UpdateUserInfoAsync(guid, adminMember);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "修改成功";
            }
            catch
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

            AdminMember adminMember = _mapper.Map<AdminMember>(baseRequest.Data);

            try
            {
                await _adminMemberAccountService.RegisterAsync(adminMember);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "建立成功";
            }
            catch
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

            AdminMember adminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (adminMember == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "找不到資料";

                return baseResponse;
            }

            try
            {
                await _adminMemberService.DeleteByGuidAsync(guid);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "刪除成功";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "刪除失敗";
            }

            return baseResponse;
        }
    }
}