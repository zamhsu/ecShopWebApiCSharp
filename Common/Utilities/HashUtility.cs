using Common.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class HashUtility
    {
        /// <summary>
        /// 建立常用雜湊
        /// </summary>
        /// <param name="data">要被計算雜湊的資料</param>
        /// <param name="hashAlgorithm">雜湊演算法</param>
        public static string CreateGeneralHash(byte[] data, GeneralHashAlgorithmEnum hashAlgorithm)
        {
            string hashAlgorithmName = Enum.GetName(typeof(GeneralHashAlgorithmEnum), hashAlgorithm);
            if (string.IsNullOrWhiteSpace(hashAlgorithmName))
            {
                throw new ArgumentNullException(nameof(hashAlgorithmName));
            }

            HashAlgorithm algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithmName);
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name");
            }

            string result = BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);

            return result;
        }

        /// <summary>
        /// 建立HmacSha256簽章
        /// </summary>
        /// <param name="signKey">簽章金鑰</param>
        /// <returns></returns>
        public static SigningCredentials CreateHmacSha256Signature(string signKey)
        {
            if (signKey.Length < 16)
            {
                throw new ArgumentException("Must longer than 16 characters");
            }

            // 建立一組對稱式加密的金鑰
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

            // 建立HmacSha256簽章
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            return signingCredentials;
        }
    }
}