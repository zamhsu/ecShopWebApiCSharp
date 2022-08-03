using Common.Helpers;
using Repository.Entities.Members;

namespace Service.Interfaces.Members
{
    public interface IAdminMemberService
    {
        /// <summary>
        /// 使用Guid取得一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<AdminMember> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<AdminMember> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 使用帳號取得一筆管理員資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        Task<AdminMember> GetByAccountAsync(string account);

        /// <summary>
        /// 取得所有管理員
        /// </summary>
        /// <returns></returns>
        Task<List<AdminMember>> GetAllAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有管理員
        /// </summary>
        /// <returns></returns>
        Task<List<AdminMember>> GetDetailAllAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有管理員
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<AdminMember> GetPagedDetailAll(int pageSize, int page);

        /// <summary>
        /// 新增一筆管理員資料
        /// </summary>
        /// <param name="adminMember">新增管理員的資料</param>
        /// <returns></returns>
        Task CreateAsync(AdminMember adminMember);

        /// <summary>
        /// 修改一筆管理員個人資料
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <param name="adminMember">修改管理員的資料</param>
        /// <returns></returns>
        Task UpdateUserInfoAsync(string guid, AdminMember adminMember);

        /// <summary>
        /// 修改一筆管理員資料
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <param name="adminMember">修改管理員的資料</param>
        /// <returns></returns>
        Task UpdateAsync(string guid, AdminMember adminMember);

        /// <summary>
        /// 使用Guid刪除一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task DeleteByGuidAsync(string guid);
    }
}