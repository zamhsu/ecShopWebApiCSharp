using WebApi.Dtos.Members;
using WebApi.Models;

namespace WebApi.Base.IServices.Members
{
    public interface IAdminMemberAccountService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        Task<AdminMemberInfoModel?> LoginAsync(string account, string rawPassword);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task LogoutAsync(string guid);

        /// <summary>
        /// 更新Token過期時間
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="expirationDate">Token過期時間</param>
        /// <returns></returns>
        Task UpdateExpirationDateAsync(string account, DateTimeOffset expirationDate);

        /// <summary>
        /// 更新登入密碼輸入錯誤次數
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="errorTimes">錯誤次數</param>
        /// <returns></returns>
        Task UpdateErrorTimesAsync(string account, int errorTimes);

        /// <summary>
        /// 鎖定帳號
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        Task LockAccount(string account);

        /// <summary>
        /// 更換密碼
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="currentRawPassword">目前明碼密碼</param>
        /// <param name="newRawPassword">新明碼密碼</param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(string account, string currentRawPassword, string newRawPassword);

        /// <summary>
        /// 帳號的密碼是否相同
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        Task<bool> IsSamePasswordAsync(string account, string rawPassword);

        /// <summary>
        /// 是否超過允許登入失敗次數
        /// </summary>
        /// <param name="currentTimes">目前登入失敗次數</param>
        /// <returns></returns>
        bool IsLegalLoginErrorTimes(int currentTimes);

        /// <summary>
        /// 帳號是否被鎖定
        /// </summary>
        /// <param name="statusPara">管理員帳號狀態參數</param>
        /// <returns></returns>
        bool IsAccountLocked(AdminMemberStatusPara statusPara);
    }
}