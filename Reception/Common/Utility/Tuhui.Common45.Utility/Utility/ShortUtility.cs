using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：ShortUtility
    /// <summary>
    /// Short类型扩展方法
    /// </summary>
    /// <remarks>
    /// Short类型扩展方法
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public static class ShortUtility
    {
        /// <summary>
        /// 将short分解成short数组
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static short[] ToArray(this short t)
        {
            List<short> returnList = new List<short>();
            if (t == 0)
            {
                returnList.Add(0);
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    short _s = (short)Math.Pow(2, i);
                    if ((t & _s) != 0)
                    {
                        returnList.Add(_s);
                    }
                }
            }
            return returnList.ToArray();
        }

        /// <summary>
        /// 将short分解成字符串数组
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string[] ToStringArray(this short t)
        {
            return t.ToArray().Select(p => p + string.Empty).ToArray();
        }

        /// <summary>
        /// 将short数组合并成一个short
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static short ToShort(this IEnumerable<short> t)
        {
            if (t == null)
            {
                return 0;
            }
            else
            {
                return t.Aggregate((p, q) => (short)(p + q));
            }
        }
    }
}
