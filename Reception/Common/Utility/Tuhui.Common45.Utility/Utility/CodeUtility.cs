using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：CodeUtility
    /// <summary>
    /// 唯一号生成
    /// </summary>
    /// <remarks>
    /// 唯一号生成
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/17        李根华           新建
    /// =======================================================================
    public static class CodeUtility
    {
        /// =======================================================================
        /// 方法名：GenerateCode
        /// <summary>
        /// 唯一号生成（含字符数字）(基于Guid，长度16)
        /// </summary>
        /// <remarks>
        /// 唯一号生成（含字符数字）(基于Guid，长度16)
        /// </remarks>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/17        李根华           新建
        /// =======================================================================
        public static string GenerateCode()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// =======================================================================
        /// 方法名：GenerateCode
        /// <summary>
        /// 唯一号生成（含字符数字）(基于Guid，长度32)
        /// </summary>
        /// <remarks>
        /// 唯一号生成（含字符数字）(基于Guid，长度32)
        /// </remarks>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/17        李根华           新建
        /// =======================================================================
        public static string GenerateCode32()
        {
            return string.Join(string.Empty, Guid.NewGuid().ToString().Split('-'));
        }

        /// =======================================================================
        /// 方法名：ConfuseCode
        /// <summary>
        /// 无序化数字字符
        /// </summary>
        /// <remarks>
        /// 无序化数字字符
        /// </remarks>
        /// <param name="id">有序的数字字符（如需定长，需左补0）</param>
        /// <param name="flag"></param>
        /// <returns>无序的数字字符（flag为true时长度是原字符长度+3）</returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public static string ConfuseCode(string id, bool flag = true)
        {
            return (new RandomId()).ConfuseId(id, id.Length, flag);

        }

        /// =======================================================================
        /// 方法名：GetRandomCode
        /// <summary>
        /// 获得随机数
        /// </summary>
        /// <remarks>
        /// 获得随机数（不保证唯一）
        /// </remarks>
        /// <param name="length">随机数长度</param>
        /// <param name="onlyNumber">是否只包含数字（默认true）</param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2012/02/06        李根华           新建
        /// =======================================================================
        public static string GetRandomCode(int length, bool onlyNumber = true)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int len = 9;

            if (onlyNumber == false)
            {
                chars = new char[] {
                                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's',
                                't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
                            };

                len = 32;
            }

            string code = string.Empty;

            for (int i = 0; i < length; i++)
            {
                //传入一个seed参数即可保证生成的随机数不同
                Random rnd = new Random(GetRandomSeed());
                code += chars[rnd.Next(0, len)].ToString();
            }

            return code;
        }

        /// <summary>
        /// 生成随机种子
        /// </summary>
        /// <returns></returns>
        private static int GetRandomSeed()
        {

            byte[] bytes = new byte[4];

            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();

            rng.GetBytes(bytes);

            return BitConverter.ToInt32(bytes, 0);

        }

    }

    internal class RandomId
    {
        private Random random;
        private String table;
        public RandomId()
        {
            random = new Random();
            table = "0123456789";
        }
        public String ConfuseId(string id, int len, bool flag)
        {
            string num = id.ToString().PadLeft(len, '0');

            int key = random.Next(1, 10),
                seed = random.Next(100);

            Caesar caesar = new Caesar(table, seed);

            num = caesar.Encode(key, num);

            if ( flag ) {
                return num
                    + key.ToString()
                    + seed.ToString().PadLeft( 2, '0' );
            } else {
                return num;
            }

        }
    }
}
