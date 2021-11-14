using WebApi.Models.Members;

namespace WebApi.Base.IServices.Members
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
        Task DeleteAsync(string guid);
    }
}