using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CandyJun.CryptoService.AesCrypto
{
    public class AesCryptoService
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncrypt(string value, string key, CipherMode cipherMode, PaddingMode paddingMode, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");

            var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Encoding.UTF8.GetBytes(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = cipherMode;
                aes.Padding = paddingMode;
                var cryptoTransform = aes.CreateEncryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        /// <summary>
        /// AES加密 - CBC模式，PKCS7填充，支持128位、192位、256位加密
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncrypt(string value, string key, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Encoding.UTF8.GetBytes(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var cryptoTransform = aes.CreateEncryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecrypt(string value, string key, CipherMode cipherMode, PaddingMode paddingMode, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");

            var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Convert.FromBase64String(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = cipherMode;
                aes.Padding = paddingMode;
                var cryptoTransform = aes.CreateDecryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }

        /// <summary>
        /// AES解密 - CBC模式，PKCS7填充，支持128位、192位、256位加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecrypt(string value, string key, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Convert.FromBase64String(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var cryptoTransform = aes.CreateDecryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncryptJava(string value, string key, CipherMode cipherMode, PaddingMode paddingMode, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");

            var kgen = Org.BouncyCastle.Security.GeneratorUtilities.GetKeyGenerator("AES");
            var secureRandom = Org.BouncyCastle.Security.SecureRandom.GetInstance("SHA1PRNG");
            secureRandom.SetSeed(Encoding.ASCII.GetBytes(key));
            kgen.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(secureRandom, 128));

            var _keyByte = kgen.GenerateKey();
             var _valueByte = Encoding.UTF8.GetBytes(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = cipherMode;
                aes.Padding = paddingMode;
                var cryptoTransform = aes.CreateEncryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecryptJava(string value, string key, CipherMode cipherMode, PaddingMode paddingMode, string iv = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("密钥不能为空。");

            var kgen = Org.BouncyCastle.Security.GeneratorUtilities.GetKeyGenerator("AES");
            var secureRandom = Org.BouncyCastle.Security.SecureRandom.GetInstance("SHA1PRNG");
            secureRandom.SetSeed(Encoding.ASCII.GetBytes(key));
            kgen.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(secureRandom, 128));

            var _keyByte = kgen.GenerateKey();
            var _valueByte = Convert.FromBase64String(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv) ? Encoding.UTF8.GetBytes(iv) : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = _keyByte;
                aes.Mode = cipherMode;
                aes.Padding = paddingMode;
                var cryptoTransform = aes.CreateDecryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
