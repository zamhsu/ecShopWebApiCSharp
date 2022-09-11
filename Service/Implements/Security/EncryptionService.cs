using Common.Enums;
using Common.Utilities;
using Service.Interfaces.Security;
using System.Text;

namespace Service.Implements.Security
{
    public class EncryptionService : IEncryptionService
    {
        /// <summary>
        /// 新增密碼雜湊
        /// </summary>
        /// <param name="password">明碼密碼</param>
        /// <param name="saltKey">雜湊鹽</param>
        /// <param name="hashAlgorithm">計算雜湊的演算法</param>
        /// <returns>Password hash</returns>
        public string CreatePasswordHash(string password, string saltKey, GeneralHashAlgorithmEnum hashAlgorithm)
        {
            byte[] encodedPassword = Encoding.UTF8.GetBytes($"{password}{saltKey}");
            string result = HashUtility.CreateGeneralHash(encodedPassword, hashAlgorithm);

            return result;
        }
    }
}