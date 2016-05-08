using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class StringHelper
    {
        /// <summary> 
        /// 加密字符串 
        /// 注意:密钥必须为８位 
        /// </summary> 
        public static string DesEncrypt(string inputString, string encryptKey)
        {
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byte[] byKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Encoding.UTF8.GetBytes(inputString);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception error)
            {
                return "";
            }
        }

        /// <summary> 
        /// 解密字符串 
        /// </summary> 
        public static string DesDecrypt(string inputString, string decryptKey)
        {
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                var byKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Convert.FromBase64String(inputString);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(byKey, iv), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = new UTF8Encoding();
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception error)
            {
                return "";
            }
        }
    }
    public static class CommonHelper
    {
        public static string GetSubString(this string str, int startIndex, int length)
        {
            return str.Substring(startIndex, str.Length > length ? length : str.Length);
        }
    }
}
