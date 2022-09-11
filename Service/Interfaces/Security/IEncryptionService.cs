using Common.Enums;

namespace Service.Interfaces.Security
{
    public interface IEncryptionService
    {
        /// <summary>
        /// 新增密碼雜湊
        /// </summary>
        /// <param name="password">明碼密碼</param>
        /// <param name="saltKey">雜湊鹽</param>
        /// <param name="hashAlgorithm">計算雜湊的演算法</param>
        /// <returns>Password hash</returns>
        string CreatePasswordHash(string password, string saltKey, GeneralHashAlgorithmEnum hashAlgorithm);
    }
}