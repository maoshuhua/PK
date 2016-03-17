using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：CommonUtility
    /// <summary>
    /// 其他功能帮助类
    /// </summary>
    /// <remarks>
    /// 其他功能帮助类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public class CommonUtility
    {
        /// <summary>
        /// 将一个匿名对象转换成一个字典
        /// </summary>
        /// <param name="obj">匿名对象</param>
        /// <returns>字典结果</returns>
        public static Dictionary<string, object> AnonymousObjectToDictionary(object obj)
        {
            var result = new Dictionary<string, object>();
            if (obj != null)
            {
                if (obj is Dictionary<string, object>)
                {
                    return obj as Dictionary<string, object>;
                }
                else
                {
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                    {
                        var value = property.GetValue(obj);
                        result.Add(property.Name, value);
                    }
                }
            }
            return result;
        }

        /// =======================================================================
        /// 方法名：GetJSTimespan
        /// <summary>
        /// 获取js的时间戳（从1970/1/1 开始的毫秒数,UTC时间）
        /// </summary>
        /// <remarks>
        /// 获取js的时间戳（从1970/1/1 开始的毫秒数,UTC时间）
        /// </remarks>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2013/06/20        李根华           新建
        /// =======================================================================
        public static long GetJSTimespan(DateTime dt)
        {
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1);
            return (long)ts.TotalMilliseconds; 

        }

        /// =======================================================================
        /// 方法名：GetDateTimeFromJS
        /// <summary>
        /// 根据js的时间戳获得本地时间
        /// </summary>
        /// <remarks>
        /// 根据js的时间戳获得本地时间
        /// </remarks>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2013/08/30        李根华           新建
        /// =======================================================================
        public static DateTime GetDateTimeFromJS(long dt)
        {
            return (new DateTime(1970, 1, 1)).AddMilliseconds(dt).ToLocalTime();
        }

        public static Dictionary<string, object> AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name, property.GetValue(htmlAttributes));
                }
            }

            return result;
        }
    }
}
