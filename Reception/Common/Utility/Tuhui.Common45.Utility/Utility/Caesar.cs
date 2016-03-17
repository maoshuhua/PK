using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：Caesar
    /// <summary>
    /// 凯撒加解密类
    /// </summary>
    /// <remarks>
    /// 凯撒加解密类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/18        李根华           新建
    /// =======================================================================
    public class Caesar
    {
        private String table;
        private int seedA = 1103515245;
        private int seedB = 12345;

        /// =======================================================================
        /// 构造函数名：Caesar
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 构造函数
        /// </remarks>
        /// <param name="table">字符表</param>
        /// <param name="seed">种子</param>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public Caesar(String table, int seed)
        {
            this.table = chaos(table, seed, table.Length);
        }

        /// =======================================================================
        /// 构造函数名：Caesar
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 构造函数
        /// </remarks>
        /// <param name="table">字符表（种子默认11）</param>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public Caesar(String table)
            : this(table, 11)
        {

        }

        /// =======================================================================
        /// 构造函数名：Caesar
        /// <summary>
        /// 构造函数（字符表默认大写英文字符，种子默认11）
        /// </summary>
        /// <remarks>
        /// 构造函数
        /// </remarks>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public Caesar()
            : this(11)
        {

        }
        /// =======================================================================
        /// 构造函数名：Caesar
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 构造函数
        /// </remarks>
        /// <param name="seed">种子（字符表默认大写英文字符）</param>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public Caesar(int seed)
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ", seed)
        {

        }
        private char dict(int i, bool reverse)
        {
            int s = table.Length, index = reverse ? s - i : i;
            return table.ToCharArray()[index];
        }
        private int dict(char c, bool reverse)
        {
            int s = table.Length, index = table.IndexOf(c);
            return reverse ? s - index : index;
        }
        private int Seed(int seed)
        {
            long temp = seed;
            return (int)((temp * seedA + seedB) & 0x7fffffffL);
        }

        private string chaos(String data, int seed, int cnt)
        {
            StringBuilder buf = new StringBuilder(data);
            char tmp; int a, b, r = data.Length;
            for (int i = 0; i < cnt; i += 1)
            {
                seed = Seed(seed); a = seed % r;
                seed = Seed(seed); b = seed % r;
                tmp = data.ToCharArray()[a];

                char[] tmp_a = data.ToCharArray();
                tmp_a[a] = data.ToCharArray()[b];
                data = new string(tmp_a);
                //buf.setCharAt((a, buf.ToString().ToCharArray()[b]); 

                char[] tmp_b = data.ToCharArray();
                tmp_b[b] = tmp;
                data = new string(tmp_b);
                //buf.setCharAt(b, tmp);  
            }
            return buf.ToString();
        }

        private String crypto(bool reverse, int key, String text)
        {
            String ret = null;
            StringBuilder buf = new StringBuilder();
            int m, s = table.Length, e = text.Length;

            for (int i = 0; i < e; i += 1)
            {
                m = dict(text.ToCharArray()[i], reverse);
                if (m < 0) break;
                m = m + key + i;
                buf.Append(dict(m % s, reverse));
            }
            if (buf.Length == e)
                ret = buf.ToString();
            return ret;
        }
        /// =======================================================================
        /// 方法名：Encode
        /// <summary>
        /// 加密
        /// </summary>
        /// <remarks>
        /// 加密
        /// </remarks>
        /// <param name="key">key值（位移值）</param>
        /// <param name="text">需要加密的文本</param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public String Encode(int key, String text)
        {
            return crypto(false, key, text);

        }

        /// =======================================================================
        /// 方法名：Decode
        /// <summary>
        /// 解密
        /// </summary>
        /// <remarks>
        /// 解密
        /// </remarks>
        /// <param name="key">key值（位移值）</param>
        /// <param name="text">需要解密的文本</param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2011/11/18        李根华           新建
        /// =======================================================================
        public String Decode(int key, String text)
        {
            return crypto(true, key, text);
        }


    }
}
