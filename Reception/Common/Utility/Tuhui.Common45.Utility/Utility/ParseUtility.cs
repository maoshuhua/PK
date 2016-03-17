using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：PareseUtility
    /// <summary>
    /// 值转换
    /// </summary>
    /// <remarks>
    /// 值转换
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public class ParseUtility
    {
        /// <summary>
        /// 字符串转换为bool类型，如果转换不了，返回bool默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ParseBoolean(string value)
        {
            return ParseBoolean(value, default(bool));
        }

        /// <summary>
        /// 字符串装换为bool类型，如果转换不了，返回指定值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ParseBoolean(string value, bool defaultValue)
        {
            var _item = false;
            if (!string.IsNullOrEmpty(value) && bool.TryParse(value, out _item))
            {
                return _item;
            }
            else
            {
                return defaultValue;
            }
        }

    }
}
