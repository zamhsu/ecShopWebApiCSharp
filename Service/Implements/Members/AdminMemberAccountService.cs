using AutoMapper;
using Common.Enums;
using Repository.Entities.Members;
using Service.Dtos.Members;
using Service.Interfaces.Members;
using Service.Interfaces.Security;

namespace Service.Implements.Members
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
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);
            if (adminMemberDto is null)
            {
                return null;
            }

            int errorTimes = adminMemberDto.ErrorTimes;
            if (IsAccountLocked((AdminMemberStatusEnum)adminMemberDto.StatusId))
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
            adminMemberDto = await _adminMemberService.GetByAccountAsync(account);

            AdminMemberInfoDto model = _mapper.Map<AdminMemberInfoDto>(adminMemberDto);

            return model;
        }

        /// <summary>
        /// 只使用Guid登入
        /// </summary>
        /// <param name="guid">使用者Guid</param>
        /// <returns></returns>
        public async Task<AdminMemberInfoDto> LoginByOnlyGuidAsync(string guid)
        {
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByGuidAsync(guid);
            if (adminMemberDto is null)
            {
                return null;
            }

            int errorTimes = 0;
            DateTimeOffset expirationDate = new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUniversalTime();
            await UpdateExpirationDateAsync(adminMemberDto.Account, expirationDate);
            await UpdateErrorTimesAsync(adminMemberDto.Account, errorTimes);
            adminMemberDto = await _adminMemberService.GetByAccountAsync(adminMemberDto.Account);

            AdminMemberInfoDto model = _mapper.Map<AdminMemberInfoDto>(adminMemberDto);

            return model;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="guid">管理員GUID</param>
        /// <returns></returns>
        public async Task<bool> LogoutAsync(string guid)
        {
            DateTimeOffset expirationDate = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)).ToUniversalTime();

            return await UpdateExpirationDateAsync(guid, expirationDate);
        }

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="adminMember">管理員註冊資料</param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(AdminMemberRegisterDto registerDto)
        {
            AdminMemberCreateDto createDto = _mapper.Map<AdminMemberCreateDto>(registerDto);

            string hashSalt = Guid.NewGuid().ToString();

            createDto.Pwd = HashPassword(createDto.Pwd, hashSalt);
            createDto.HashSalt = hashSalt;

            return await _adminMemberService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新Token過期時間
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="expirationDate">Token過期時間</param>
        /// <returns></returns>
        public async Task<bool> UpdateExpirationDateAsync(string account, DateTimeOffset expirationDate)
        {
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);
            if (adminMemberDto is null)
            {
                throw new ArgumentNullException(nameof(adminMemberDto));
            }

            adminMemberDto.ExpirationDate = expirationDate;

            AdminMemberUpdateDto updateDto = _mapper.Map<AdminMemberUpdateDto>(adminMemberDto);

            return await _adminMemberService.UpdateAsync(updateDto);
        }

        /// <summary>
        /// 更新登入密碼輸入錯誤次數
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="errorTimes">錯誤次數</param>
        /// <returns></returns>
        public async Task<bool> UpdateErrorTimesAsync(string account, int errorTimes)
        {
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);
            if (adminMemberDto is null)
            {
                throw new ArgumentNullException(nameof(adminMemberDto));
            }

            adminMemberDto.ErrorTimes = errorTimes;
            adminMemberDto.LastLoginDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime();

            AdminMemberUpdateDto updateDto = _mapper.Map<AdminMemberUpdateDto>(adminMemberDto);

            return await _adminMemberService.UpdateAsync(updateDto);
        }

        /// <summary>
        /// 鎖定帳號
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public async Task<bool> LockAccount(string account)
        {
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);
            if (adminMemberDto is null)
            {
                throw new ArgumentNullException(nameof(adminMemberDto));
            }

            adminMemberDto.StatusId = (int)AdminMemberStatusEnum.Lock;

            AdminMemberUpdateDto updateDto = _mapper.Map<AdminMemberUpdateDto>(adminMemberDto);

            return await _adminMemberService.UpdateAsync(updateDto);
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
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);
            if (adminMemberDto is null)
            {
                return false;
            }

            bool isCurrenPasswordCorrect = await IsSamePasswordAsync(account, currentRawPassword);
            if (!isCurrenPasswordCorrect)
            {
                return false;
            }

            string hashedNewPassword = HashPassword(newRawPassword, adminMemberDto.HashSalt);

            adminMemberDto.Pwd = hashedNewPassword;

            AdminMemberUpdateDto updateDto = _mapper.Map<AdminMemberUpdateDto>(adminMemberDto);

            return await _adminMemberService.UpdateAsync(updateDto);
        }

        /// <summary>
        /// 帳號的密碼是否相同
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="rawPassword">明碼密碼</param>
        /// <returns></returns>
        public async Task<bool> IsSamePasswordAsync(string account, string rawPassword)
        {
            AdminMemberDto adminMemberDto = await _adminMemberService.GetByAccountAsync(account);

            if (adminMemberDto is null)
            {
                return false;
            }

            string hashedPassword = HashPassword(rawPassword, adminMemberDto.HashSalt);

            return adminMemberDto.Pwd == hashedPassword;
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
            return status.Equals(AdminMemberStatusEnum.Lock);
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