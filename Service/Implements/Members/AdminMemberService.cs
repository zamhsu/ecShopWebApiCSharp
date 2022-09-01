using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Common.Helpers;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Entities.Members;
using Repository.Interfaces;
using Service.Dtos.Members;
using Service.Interfaces.Members;

namespace Service.Implements.Members
{
    public class AdminMemberService : IAdminMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<AdminMember> _logger;

        public AdminMemberService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<AdminMember> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<AdminMemberDto> GetByGuidAsync(string guid)
        {
            AdminMember adminMember = await _unitOfWork.Repository<AdminMember>()
                .GetAsync(q => q.Guid.Equals(guid));

            AdminMemberDto dto = _mapper.Map<AdminMemberDto>(adminMember);

            return dto;
        }

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<AdminMemberDetailDto> GetDetailByGuidAsync(string guid)
        {
            AdminMember adminMember = await _unitOfWork.Repository<AdminMember>().GetAll()
                .Include(q => q.AdminMemberStatus)
                .FirstOrDefaultAsync(q => q.Guid.Equals(guid));

            AdminMemberDetailDto dto = _mapper.Map<AdminMemberDetailDto>(adminMember);

            return dto;
        }

        /// <summary>
        /// 使用帳號取得一筆管理員資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public async Task<AdminMemberDto> GetByAccountAsync(string account)
        {
            AdminMember adminMember = await _unitOfWork.Repository<AdminMember>()
                .GetAsync(q => q.Account.Equals(account));

            AdminMemberDto dto = _mapper.Map<AdminMemberDto>(adminMember);

            return dto;
        }

        /// <summary>
        /// 取得所有管理員
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminMemberDto>> GetAllAsync()
        {
            IQueryable<AdminMember> query = _unitOfWork.Repository<AdminMember>().GetAll();
            List<AdminMember> adminMembers = await query.ToListAsync();

            List<AdminMemberDto> dtos = _mapper.Map<List<AdminMemberDto>>(adminMembers);

            return dtos;
        }

        /// <summary>
        /// 取得包含關聯性資料的所有管理員
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminMemberDetailDto>> GetDetailAllAsync()
        {
            IQueryable<AdminMember> query = _unitOfWork.Repository<AdminMember>().GetAll()
                .Include(q => q.AdminMemberStatus);
            List<AdminMember> adminMembers = await query.ToListAsync();

            List<AdminMemberDetailDto> dtos = _mapper.Map<List<AdminMemberDetailDto>>(adminMembers);

            return dtos;
        }

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有管理員
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<AdminMemberDetailDto> GetPagedDetailAll(int pageSize, int page)
        {
            IQueryable<AdminMember> query = _unitOfWork.Repository<AdminMember>().GetAll()
                .Include(q => q.AdminMemberStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<AdminMember> adminMembers = query.ToPagedList(pageSize, page);

            PagedList<AdminMemberDetailDto> dtos = _mapper.Map<PagedList<AdminMemberDetailDto>>(adminMembers);

            return dtos;
        }

        /// <summary>
        /// 新增一筆管理員資料
        /// </summary>
        /// <param name="createDto">新增管理員的資料</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(AdminMemberCreateDto createDto)
        {
            if (createDto is null)
            {
                _logger.LogInformation("[Create] AdminMemberCreateDto can not be null");
                throw new ArgumentNullException(nameof(createDto));
            }

            AdminMember adminMember = _mapper.Map<AdminMember>(createDto);

            adminMember.Guid = Guid.NewGuid().ToString();
            adminMember.StatusId = (int)AdminMemberStatusEnum.OK;
            adminMember.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            await _unitOfWork.Repository<AdminMember>().CreateAsync(adminMember);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆管理員個人資料
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <param name="adminMember">修改管理員的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateUserInfoAsync(AdminMemberUserInfoDto userInfoDto)
        {
            AdminMember entity = await _unitOfWork.Repository<AdminMember>()
                .GetAsync(q => q.Guid.Equals(userInfoDto.Guid));

            if (entity is null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{userInfoDto.Guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            entity.UserName = userInfoDto.UserName;
            entity.Email = userInfoDto.Email;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<AdminMember>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改一筆管理員資料
        /// </summary>
        /// <param name="updateDto">修改管理員的資料</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(AdminMemberUpdateDto updateDto)
        {
            AdminMember entity = await _unitOfWork.Repository<AdminMember>()
                .GetAsync(q => q.Guid.Equals(updateDto.Guid));

            if (entity is null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{updateDto.Guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            entity.UserName = updateDto.UserName;
            entity.Email = updateDto.Email;
            entity.Pwd = updateDto.Pwd;
            entity.StatusId = updateDto.StatusId;
            entity.ErrorTimes = updateDto.ErrorTimes;
            entity.LastLoginDate = updateDto.LastLoginDate;
            entity.ExpirationDate = updateDto.ExpirationDate;
            entity.IsMaster = updateDto.IsMaster;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<AdminMember>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 使用Guid刪除一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<bool> DeleteByGuidAsync(string guid)
        {
            AdminMember entity = await _unitOfWork.Repository<AdminMember>()
                .GetAsync(q => q.Guid.Equals(guid));

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.StatusId = (int)AdminMemberStatusEnum.Delete;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            _unitOfWork.Repository<AdminMember>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}