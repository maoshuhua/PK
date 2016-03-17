using System;
using System.Collections.Generic;
using System.Linq;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：IEnumerableExtension
    /// <summary>
    /// 迭代器类型方法扩展
    /// </summary>
    /// <remarks>
    /// 迭代器类型方法扩展
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 扩展ForEach方法
        /// </summary>
        /// <typeparam name="T">迭代器泛型</typeparam>
        /// <param name="ienum">迭代器实例</param>
        /// <param name="action">方法委托</param>
        public static void ForEach<T>(this IEnumerable<T> ienum, Action<T> action)
        {
            if (ienum == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in ienum)
                action(item);
        }

        /// <summary>
        /// 扩展Contains方法(检测是否包含指定集合中的任意元素)
        /// </summary>
        /// <param name="ienum">原始集合</param>
        /// <param name="items">目标集合</param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> ienum, List<string> items)
        {
            if (ienum == null) return false;

            foreach (var item in items)
            {
                if (ienum.Contains(item)) return true;
            }

            return false;
        }
    }
}