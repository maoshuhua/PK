using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tuhui.OH.Utility
{
    public class _Decrypt
    {
        private const string defaultKey = "BOC_PLFP"; //默认密钥
        private static byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//des 向量

        /// <summary>
        ///  des解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。默认公钥解密字符串defaultKey</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey = defaultKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.Mode = CipherMode.ECB;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public static string DecryptParam(string strValue, string strKey)
        {
            string rtn = string.Empty;
            if (string.IsNullOrEmpty(strValue) == false)
            {
                try
                {
                    //strValue = System.Web.HttpUtility.UrlDecode(strValue);

                    strValue = Decrypt(strValue, strKey);
                    rtn = strValue;
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
    }
}
