using System.Security.Cryptography;

namespace WebApi.Utilities
{
    public class HashUtility
    {
        /// <summary>
        /// 建立雜湊
        /// </summary>
        /// <param name="data">要被計算雜湊的資料</param>
        /// <param name="hashAlgorithm">雜湊演算法</param>
        public static string CreateHash(byte[] data, string hashAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(hashAlgorithm))
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            HashAlgorithm algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm);
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name");
            }

            string result = BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);

            return result;
        }
    }
}