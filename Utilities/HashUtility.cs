using System.Security.Cryptography;
using WebApi.Models;

namespace WebApi.Utilities
{
    public class HashUtility
    {
        /// <summary>
        /// 建立常用雜湊
        /// </summary>
        /// <param name="data">要被計算雜湊的資料</param>
        /// <param name="hashAlgorithm">雜湊演算法</param>
        public static string CreateGeneralHash(byte[] data, GeneralHashAlgorithmPara hashAlgorithm)
        {
            string hashAlgorithmName = Enum.GetName(typeof(GeneralHashAlgorithmPara), hashAlgorithm);
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
    }
}