using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Base.IServices.Members;
using WebApi.Dtos;
using WebApi.Extensions;
using WebApi.Models;
using WebApi.Models.Members;

namespace WebApi.Base.Services.Members
{
    public class AdminMemberService : IAdminMemberService
    {
        private readonly IRepository<AdminMember> _adminMemberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<AdminMember> _logger;

        public AdminMemberService(IRepository<AdminMember> adminMemberRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppLogger<AdminMember> logger)
        {
            _adminMemberRepository = adminMemberRepository;
            _unitOfWork = unitOfWork;
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
        /// 使用Guid取得一筆包含關聯性資料的管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<AdminMember> GetDetailByGuidAsync(string guid)
        {
            AdminMember adminMember = await _adminMemberRepository.GetAll()
                .Include(q => q.AdminMemberStatus)
                .FirstOrDefaultAsync(q => q.Guid == guid);

            return adminMember;
        }

        /// <summary>
        /// 使用帳號取得一筆管理員資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public async Task<AdminMember> GetByAccountAsync(string account)
        {
            AdminMember adminMember = await _adminMemberRepository.GetAsync(q => q.Account == account);

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
        /// 取得包含關聯性資料的所有管理員
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminMember>> GetDetailAllAsync()
        {
            IQueryable<AdminMember> query = _adminMemberRepository.GetAll()
                .Include(q => q.AdminMemberStatus);
            List<AdminMember> adminMembers = await query.ToListAsync();

            return adminMembers;
        }

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有管理員
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        public PagedList<AdminMember> GetPagedDetailAll(int pageSize, int page)
        {
            IQueryable<AdminMember> query = _adminMemberRepository.GetAll()
                .Include(q => q.AdminMemberStatus);

            query = query.OrderByDescending(q => q.Id);

            PagedList<AdminMember> adminMembers = query.ToPagedList(pageSize, page);

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
            adminMember.StatusId = (int)AdminMemberStatusPara.OK;
            adminMember.CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                await _adminMemberRepository.CreateAsync(adminMember);
                await _unitOfWork.SaveChangesAsync();
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
            AdminMember entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            entity.UserName = adminMember.UserName;
            entity.Email = adminMember.Email;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                _adminMemberRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
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
            AdminMember entity = await GetByGuidAsync(guid);

            if (entity == null)
            {
                _logger.LogInformation($"[Update] AdminMember is not existed (Guid:{guid})");
                throw new ArgumentNullException(nameof(AdminMember));
            }

            entity.UserName = adminMember.UserName;
            entity.Email = adminMember.Email;
            entity.Pwd = adminMember.Pwd;
            entity.StatusId = adminMember.StatusId;
            entity.ErrorTimes = adminMember.ErrorTimes;
            entity.LastLoginDate = adminMember.LastLoginDate;
            entity.ExpirationDate = adminMember.ExpirationDate;
            entity.IsMaster = adminMember.IsMaster;
            entity.UpdateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            try
            {
                _adminMemberRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
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
        public async Task DeleteByGuidAsync(string guid)
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
                _adminMemberRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}