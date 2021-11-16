using AutoMapper;
using WebApi.Base.IServices.Members;
using WebApi.Base.IServices.Security;
using WebApi.Dtos.Members;
using WebApi.Models;
using WebApi.Models.Members;

namespace WebApi.Base.Services.Members
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
        public async Task<AdminMemberInfoModel?> LoginAsync(string account, string rawPassword)
        {
            AdminMember adminMember = await _adminMemberService.GetByAccountAsync(account);
            if (adminMember == null)
            {
                return null;
            }

            int errorTimes = adminMember.ErrorTimes;
            if (IsAccountLocked((AdminMemberStatusPara)adminMember.StatusId))
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

            AdminMemberInfoModel model = _mapper.Map<AdminMemberInfoModel>(adminMember);

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

            try
            {
                await UpdateExpirationDateAsync(guid, expirationDate);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _adminMemberService.CreateAsync(adminMember);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
            }
            catch
            {
                throw;
            }
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

            try
            {
                await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
            }
            catch
            {
                throw;
            }
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

            adminMember.StatusId = (int)AdminMemberStatusPara.Lock;

            try
            {
                await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
            }
            catch
            {
                throw;
            }
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

            try
            {
                adminMember.Pwd = hashedNewPassword;
                await _adminMemberService.UpdateAsync(adminMember.Guid, adminMember);
            }
            catch
            {
                throw;
            }

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
        public bool IsAccountLocked(AdminMemberStatusPara statusPara)
        {
            return statusPara == AdminMemberStatusPara.Lock;
        }

        /// <summary>
        /// 密碼計算雜湊
        /// </summary>
        /// <param name="rawPassword">明碼密碼</param>
        /// <param name="saltKey">雜湊鹽</param>
        /// <returns></returns>
        private string HashPassword(string rawPassword, string saltKey)
        {
            string hashAlgorithm = Enum.GetName(typeof(HashAlgorithmPara), HashAlgorithmPara.SHA256);
            string hashedPassword = _encryptionService.CreatePasswordHash(rawPassword, saltKey, hashAlgorithm);

            return hashedPassword;
        }
    }
}