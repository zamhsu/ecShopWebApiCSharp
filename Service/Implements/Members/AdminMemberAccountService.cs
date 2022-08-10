using AutoMapper;
using Common.Enums;
using Repository.Entities.Members;
using Service.Dtos.Members;
using Service.Interfaces.Members;
using Service.Interfaces.Security;

namespace Service.Implments.Members
{
    public class AdminMemberAccountService : IAdminMemberAccountService
    {
        private readonly IAdminMemberService _adminMemberService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public AdminMemberAccountService(IAdminMemberService adminMemberService,
            IEncryptionService encryptionService,
            IMapper mapper)
        {
            _adminMemberService = adminMemberService;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        public async Task<AdminMemberInfoDto> LoginAsync(string account, string rawPassword)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                return null;
            }

            int errorTimes = adminMember.ErrorTimes;
            if (IsAccountLocked((AdminMemberStatusEnum)adminMember.StatusId))
            {
                errorTimes = errorTimes + 1;
                await UpdateErrorTimesAsync(account, errorTimes);
                return null;
            }

            if (!IsLegalLoginErrorTimes(errorTimes))
            {
                errorTimes = errorTimes + 1;
                await UpdateErrorTimesAsync(account, errorTimes);
                return null;
            }

            bool isSamePassword = await IsSamePasswordAsync(account, rawPassword);
            if (!isSamePassword)
            {
                errorTimes = errorTimes + 1;
                await UpdateErrorTimesAsync(account, errorTimes);
                return null;
            }

            errorTimes = 0;
            DateTimeOffset expirationDate = new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUniversalTime();
            await UpdateExpirationDateAsync(account, expirationDate);
            await UpdateErrorTimesAsync(account, errorTimes);
            adminMember = await _adminMemberService.GetByAccountAsync(account);

            AdminMemberInfoDto model = _mapper.Map<AdminMemberInfoDto>(adminMember);

            return model;
        }

        /// <summary>
        /// 只使用Guid登入
        /// </summary>
        /// <param name="guid">使用者Guid</param>
        /// <returns></returns>
        public async Task<AdminMemberInfoDto> LoginByOnlyGuidAsync(string guid)
        {
            AdminMember adminMember = await _adminMemberService.GetByGuidAsync(guid);
            if (adminMember == null)
            {
                return null;
            }

            int errorTimes = 0;
            DateTimeOffset expirationDate = new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUniversalTime();
            await UpdateExpirationDateAsync(adminMember.Account, expirationDate);
            await UpdateErrorTimesAsync(adminMember.Account, errorTimes);
            adminMember = await _adminMemberService.GetByAccountAsync(adminMember.Account);

            AdminMemberInfoDto model = _mapper.Map<AdminMemberInfoDto>(adminMember);

            return model;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task LogoutAsync(string guid)
        {
            DateTimeOffset expirationDate = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)).ToUniversalTime();

            await UpdateExpirationDateAsync(guid, expirationDate);
        }

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="adminMember">管理員資料</param>
        /// <returns></returns>
        public async Task RegisterAsync(AdminMember adminMember)
        {
            string hashSalt = Guid.NewGuid().ToString();

            adminMember.Pwd = HashPassword(adminMember.Pwd, hashSalt);
            adminMember.HashSalt = hashSalt;

            await _adminMemberService.CreateAsync(adminMember);
        }

        /// <summary>
        /// 更新Token過期時間
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="expirationDate">Token過期時間</param>
        /// <returns></returns>
        public async Task UpdateExpirationDateAsync(string account, DateTimeOffset expirationDate)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                throw new ArgumentNullException(nameof(adminMember));
            }

            adminMember.ExpirationDate = expirationDate;

            await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
        }

        /// <summary>
        /// 更新登入密碼輸入錯誤次數
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="errorTimes">錯誤次數</param>
        /// <returns></returns>
        public async Task UpdateErrorTimesAsync(string account, int errorTimes)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                throw new ArgumentNullException(nameof(adminMember));
            }

            adminMember.ErrorTimes = errorTimes;
            adminMember.LastLoginDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
        }

        /// <summary>
        /// 鎖定帳號
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public async Task LockAccount(string account)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                throw new ArgumentNullException(nameof(adminMember));
            }

            adminMember.StatusId = (int)AdminMemberStatusEnum.Lock;

            await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
        }

        /// <summary>
        /// 更換密碼
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="currentRawPassword">目前明碼密碼</param>
        /// <param name="newRawPassword">新明碼密碼</param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(string account, string currentRawPassword, string newRawPassword)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                return false;
            }

            bool isCurrenPasswordCorrect = await IsSamePasswordAsync(account, currentRawPassword);
            if (!isCurrenPasswordCorrect)
            {
                return false;
            }

            string hashedNewPassword = HashPassword(newRawPassword, adminMember.HashSalt);

            adminMember.Pwd = hashedNewPassword;
            await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);

            return true;
        }

        /// <summary>
        /// 帳號的密碼是否相同
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        public async Task<bool> IsSamePasswordAsync(string account, string rawPassword)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);

            if (adminMember == null)
            {
                return false;
            }

            string hashedPassword = HashPassword(rawPassword, adminMember.HashSalt);

            return adminMember.Pwd == hashedPassword;
        }

        /// <summary>
        /// 是否超過允許登入失敗次數
        /// </summary>
        /// <param name="currentTimes">目前登入失敗次數</param>
        /// <returns></returns>
        public bool IsLegalLoginErrorTimes(int currentTimes)
        {
            int allowTimes = 5;

            return currentTimes <= allowTimes;
        }

        /// <summary>
        /// 帳號是否被鎖定
        /// </summary>
        /// <param name="statusPara">管理員帳號狀態參數</param>
        /// <returns></returns>
        public bool IsAccountLocked(AdminMemberStatusEnum status)
        {
            return status == AdminMemberStatusEnum.Lock;
        }

        /// <summary>
        /// 密碼計算雜湊
        /// </summary>
        /// <param name="rawPassword">明碼密碼</param>
        /// <param name="saltKey">雜湊鹽</param>
        /// <returns></returns>
        private string HashPassword(string rawPassword, string saltKey)
        {
            string hashedPassword = _encryptionService.CreatePasswordHash(rawPassword, saltKey, GeneralHashAlgorithmEnum.SHA256);

            return hashedPassword;
        }
    }
}