using System;
using System.Security.Cryptography;
using System.Text;

namespace Lightning.Cryptography
{
    /// <summary>
    /// AES加密解密类
    /// </summary>
    public class AESHelper
    {
        /// <summary>
        /// AES Base64解密
        /// </summary>
        /// <param name="value">密文</param>
        /// <param name="key">16位密钥，需和加密时的密钥保持一致</param>
        /// <param name="iv">16位向量，需和加密时的向量保持一致</param>
        /// <returns>AES解密之后的明文</returns>
        public static string AesDecrypt(string value, string key, string iv)
        {
            var valueByte = Convert.FromBase64String(value);
            Aes aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            var cryptoTransform = aes.CreateDecryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(valueByte, 0, valueByte.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES Base64加密
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="key">16位密钥，需和解密时的密钥保持一致</param>
        /// <param name="iv">16位向量，需和解密时的向量保持一致</param>
        /// <returns>AES加密之后的密文</returns>
        public static string AesEncrypt(string value, string key, string iv)
        {
            var valueByte = Encoding.UTF8.GetBytes(value);
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    var cryptoTransform = aes.CreateEncryptor();
                    var resultArray = cryptoTransform.TransformFinalBlock(valueByte, 0, valueByte.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
