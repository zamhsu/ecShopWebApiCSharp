using Common.Helpers;
using Repository.Entities.Members;
using Service.Dtos.Members;

namespace Service.Interfaces.Members
{
    public interface IAdminMemberService
    {
        /// <summary>
        /// 使用Guid取得一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<AdminMemberDto> GetByGuidAsync(string guid);

        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<AdminMemberDetailDto> GetDetailByGuidAsync(string guid);

        /// <summary>
        /// 使用帳號取得一筆管理員資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        Task<AdminMemberDto> GetByAccountAsync(string account);

        /// <summary>
        /// 取得所有管理員
        /// </summary>
        /// <returns></returns>
        Task<List<AdminMemberDto>> GetAllAsync();

        /// <summary>
        /// 取得包含關聯性資料的所有管理員
        /// </summary>
        /// <returns></returns>
        Task<List<AdminMemberDetailDto>> GetDetailAllAsync();

        /// <summary>
        /// 取得分頁後包含關聯性資料的所有管理員
        /// </summary>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        PagedList<AdminMemberDetailDto> GetPagedDetailAll(int pageSize, int page);

        /// <summary>
        /// 新增一筆管理員資料
        /// </summary>
        /// <param name="createDto">新增管理員的資料</param>
        /// <returns></returns>
        Task<bool> CreateAsync(AdminMemberCreateDto createDto);

        /// <summary>
        /// 修改一筆管理員個人資料
        /// </summary>
        /// <param name="userInfoDto">修改管理員的資料</param>
        /// <returns></returns>
        Task<bool> UpdateUserInfoAsync(AdminMemberUserInfoDto userInfoDto);

        /// <summary>
        /// 修改一筆管理員資料
        /// </summary>
        /// <param name="updateDto">修改管理員的資料</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(AdminMemberUpdateDto updateDto);

        /// <summary>
        /// 使用Guid刪除一筆管理員
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<bool> DeleteByGuidAsync(string guid);
    }
}