using Common.Enums;
using Repository.Entities.Members;
using Service.Dtos.Members;

namespace Service.Interfaces.Members
{
    public interface IAdminMemberAccountService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        Task<AdminMemberInfoDto> LoginAsync(string account, string rawPassword);

        /// <summary>
        /// 只使用Guid登入
        /// </summary>
        /// <param name="guid">使用者Guid</param>
        /// <returns></returns>
        Task<AdminMemberInfoDto> LoginByOnlyGuidAsync(string guid);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        Task<bool> LogoutAsync(string guid);

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="registerDto">管理員資料</param>
        /// <returns></returns>
        Task<bool> RegisterAsync(AdminMemberRegisterDto registerDto);

        /// <summary>
        /// 更新Token過期時間
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="expirationDate">Token過期時間</param>
        /// <returns></returns>
        Task<bool> UpdateExpirationDateAsync(string account, DateTimeOffset expirationDate);

        /// <summary>
        /// 更新登入密碼輸入錯誤次數
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="errorTimes">錯誤次數</param>
        /// <returns></returns>
        Task<bool> UpdateErrorTimesAsync(string account, int errorTimes);

        /// <summary>
        /// 鎖定帳號
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        Task<bool> LockAccount(string account);

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
        bool IsAccountLocked(AdminMemberStatusEnum status);
    }
}