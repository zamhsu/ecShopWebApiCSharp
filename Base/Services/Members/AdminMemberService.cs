using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Members;
using WebApi.Models;
using WebApi.Models.Members;

namespace WebApi.Base.Services.Members
{
    public class AdminMemberService : IAdminMemberService
    {
        private readonly IRepository<AdminMember> _adminMemberRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<AdminMember> _logger;

        public AdminMemberService(IRepository<AdminMember> adminMemberRepository,
            IMapper mapper,
            IAppLogger<AdminMember> logger)
        {
            _adminMemberRepository = adminMemberRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 使用Guid取得一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<AdminMember> GetByGuidAsync(string guid)
        {
            AdminMember adminMember = await _adminMemberRepository.GetAsync(q => q.Guid == guid);

            return adminMember;
        }

        /// <summary>
        /// 取得所有管理員
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminMember>> GetAllAsync()
        {
            IQueryable<AdminMember> query = _adminMemberRepository.GetAll();
            List<AdminMember> adminMembers = await query.ToListAsync();

            return adminMembers;
        }

        /// <summary>
        /// 新增一筆管理員資料
        /// </summary>
        /// <param name="adminMember">新增管理員的資料</param>
        /// <returns></returns>
        public async Task CreateAsync(AdminMember adminMember)
        {
            if (adminMember == null)
            {
                _logger.LogInformation("[Create] AdminMember can not be null");
                new ArgumentNullException(nameof(adminMember));
            }

            adminMember.Guid = Guid.NewGuid().ToString();
            adminMember.HashSalt = Guid.NewGuid().ToString();
            adminMember.StatusId = (int)AdminMemberStatusPara.OK;
            adminMember.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _adminMemberRepository.CreateAsync(adminMember);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆管理員個人資料
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <param name="adminMember">修改管理員的資料</param>
        /// <returns></returns>
        public async Task UpdateUserInfoAsync(string guid, AdminMember adminMember)
        {
            AdminMember enity = await GetByGuidAsync(guid);

            if (enity == null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            enity.UserName = adminMember.UserName;
            enity.Email = adminMember.Email;
            enity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _adminMemberRepository.UpdateAsync(enity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改一筆管理員資料
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <param name="adminMember">修改管理員的資料</param>
        /// <returns></returns>
        public async Task UpdateAsync(string guid, AdminMember adminMember)
        {
            AdminMember enity = await GetByGuidAsync(guid);

            if (enity == null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            enity.UserName = adminMember.UserName;
            enity.Email = adminMember.Email;
            enity.Pwd = adminMember.Pwd;
            enity.StatusId = adminMember.StatusId;
            enity.ErrorTimes = adminMember.ErrorTimes;
            enity.LastLoginDate = adminMember.LastLoginDate;
            enity.ExpirationDate = adminMember.ExpirationDate;
            enity.IsMaster = adminMember.IsMaster;
            enity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _adminMemberRepository.UpdateAsync(enity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 使用Guid刪除一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task DeleteAsync(string guid)
        {
            AdminMember entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Delete] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(entity));
            }

            entity.StatusId = (int)AdminMemberStatusPara.Delete;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _adminMemberRepository.UpdateAsync(entity);
            }
            catch
            {
                throw;
            }
        }
    }
}