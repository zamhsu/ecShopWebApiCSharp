using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Base.IServices.Members;
using WebApi.Core;
using WebApi.Dtos;
using WebApi.Dtos.Members;
using WebApi.Dtos.ViewModel;
using WebApi.Models.Members;

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
        public ActionResult<BaseResponse<AdminMemberGetAdminMemberViewModel>> GetAdminMember([FromQuery] PageQueryString pageQueryString)
        {
            BaseResponse<AdminMemberGetAdminMemberViewModel> baseResponse = new BaseResponse<AdminMemberGetAdminMemberViewModel>();

            PagedList<AdminMember> pagedList = _adminMemberService.GetPagedDetailAll(pageQueryString.PageSize, pageQueryString.Page);
            List<AdminMemberDisplayModel> adminMemberDisplays = _mapper.Map<List<AdminMemberDisplayModel>>(pagedList.PagedData);
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
        public async Task<ActionResult<BaseResponse<AdminMemberDisplayModel>>> GetAdminMember(string guid)
        {
            BaseResponse<AdminMemberDisplayModel> baseResponse = new BaseResponse<AdminMemberDisplayModel>();

            AdminMember adminMember = await _adminMemberService.GetDetailByGuidAsync(guid);
            AdminMemberDisplayModel adminMemberDisplay = _mapper.Map<AdminMemberDisplayModel>(adminMember);

            if (adminMember == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "????????????";

                return baseResponse;
            }

            baseResponse.IsSuccess = true;
            baseResponse.Data = adminMemberDisplay;

            return baseResponse;
        }

        [HttpPut("api/admin/adminMember/{guid}/userInfo")]
        public async Task<ActionResult<BaseResponse<AdminMember>>> PutAdminMemberInfo(string guid, BaseRequest<UpdateAdminMemberInfoModel> baseRequest)
        {
            BaseResponse<AdminMember> baseResponse = new BaseResponse<AdminMember>();

            AdminMember existedAdminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (existedAdminMember == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "???????????????";

                return baseResponse;
            }

            AdminMember adminMember = _mapper.Map<AdminMember>(baseRequest.Data);

            try
            {
                await _adminMemberService.UpdateUserInfoAsync(guid, adminMember);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "????????????";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "????????????";
            }

            return baseResponse;
        }

        [HttpPost("api/admin/adminMember")]
        public async Task<ActionResult<BaseResponse<AdminMember>>> PostAdminMember(BaseRequest<CreateAdminMemberModel> baseRequest)
        {
            BaseResponse<AdminMember> baseResponse = new BaseResponse<AdminMember>();

            AdminMember adminMember = _mapper.Map<AdminMember>(baseRequest.Data);

            try
            {
                await _adminMemberAccountService.RegisterAsync(adminMember);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "????????????";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "????????????";
            }

            return baseResponse;
        }

        [HttpDelete("api/admin/adminMember/{guid}")]
        public async Task<ActionResult<BaseResponse<AdminMember>>> DeleteAdminMember(string guid)
        {
            BaseResponse<AdminMember> baseResponse = new BaseResponse<AdminMember>();

            AdminMember adminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (adminMember == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "???????????????";

                return baseResponse;
            }

            try
            {
                await _adminMemberService.DeleteByGuidAsync(guid);

                baseResponse.IsSuccess = true;
                baseResponse.Message = "????????????";
            }
            catch
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "????????????";
            }

            return baseResponse;
        }
    }
}