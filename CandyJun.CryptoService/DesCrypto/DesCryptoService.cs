using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CandyJun.CryptoService.DesCrypto
{
    public class DesCryptoService
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="code">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key)
        {
            string iv = new string(key.ToCharArray().Reverse().ToArray());
            return DesEncrypt(code, key, iv);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="code">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(code);
            des.Key = Encoding.ASCII.GetBytes(key);
            des.IV = Encoding.ASCII.GetBytes(iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ms.Dispose();
            cs.Dispose();
            //ret.ToString();
            return ret.ToString();
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key)
        {
            string iv = new string(key.ToCharArray().Reverse().ToArray());
            return DesDecrypt(code, key, iv);
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[code.Length / 2];
            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(key);
            des.IV = Encoding.ASCII.GetBytes(iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            cs.Dispose();
            StringBuilder ret = new StringBuilder();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
