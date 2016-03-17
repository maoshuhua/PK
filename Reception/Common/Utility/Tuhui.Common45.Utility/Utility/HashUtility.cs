using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：HashUtility
    /// <summary>
    /// 哈希加密类
    /// </summary>
    /// <remarks>
    /// 哈希加密类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2012/04/24        李根华           新建
    /// =======================================================================
    public static class HashUtility
    {
        /// =======================================================================
        /// 方法名：GetKey
        /// <summary>
        /// 获得加密用key
        /// </summary>
        /// <remarks>
        /// 获得加密用key
        /// </remarks>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2012/04/24        李根华           新建
        /// =======================================================================
        public static string GetKey()
        {
            RNGCryptoServiceProvider cryptoProvider =
                new RNGCryptoServiceProvider();
            byte[] key = new byte[16];
            cryptoProvider.GetBytes(key);
            return Convert.ToBase64String(key);
        }
        
        /// =======================================================================
        /// 方法名：EncodeString
        /// <summary>
        /// 加密字符串（使用HMAC-SHA1算法）
        /// </summary>
        /// <remarks>
        /// 加密字符串
        /// </remarks>
        /// <param name="source">需要hash加密的字符串</param>
        /// <param name="key">加密用的Key(需要保存，下次验证时需要)(base6编码的)</param>
        /// <returns>加密后的字符串</returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2012/04/24        李根华           新建
        /// =======================================================================
        public static string EncodeString(string source, string key)
        {
            if (source == null)
                return null;

            //byte[] passwordBytes = Encoding.Unicode.GetBytes(source);
            //byte[] keyBytes = Convert.FromBase64String(key);
            //byte[] keyedBytes = new byte[passwordBytes.Length + keyBytes.Length];
            //Array.Copy(keyBytes, keyedBytes, keyBytes.Length);
            //Array.Copy(passwordBytes, 0, keyedBytes, keyBytes.Length, passwordBytes.Length);

            //return HashPasswordBytes(keyBytes, keyedBytes);

            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Convert.FromBase64String(key);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(source); //要进行签名的基础字符串
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes); 

        }

        /// =======================================================================
        /// 方法名：CheckString
        /// <summary>
        /// 验证字符
        /// </summary>
        /// <remarks>
        /// 验证字符
        /// </remarks>
        /// <param name="source">原始字符串</param>
        /// <param name="encyptString">加密的字符串</param>
        /// <param name="key">加密时的Key</param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2012/04/24        李根华           新建
        /// =======================================================================
        public static bool CheckString(string source, string encyptString, string key)
        {
            source = EncodeString(source, key);
            return source == encyptString;
        }
        

        //private static string HashPasswordBytes(byte[] key, byte[] bytes)
        //{
        //    HashAlgorithm hash = HashAlgorithm.Create();

        //    if (hash is KeyedHashAlgorithm)
        //    {
        //        KeyedHashAlgorithm keyedHash = hash as KeyedHashAlgorithm;
        //        keyedHash.Key = key;
        //    }
        //    return Convert.ToBase64String(hash.ComputeHash(bytes));
        //}


        /// <summary>
        /// 使用MD5进行hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMD5Hash(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
