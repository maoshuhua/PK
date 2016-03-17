using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：NameValueExtension
    /// <summary>
    /// NameValueCollection类型扩展类
    /// </summary>
    /// <remarks>
    /// NameValueCollection类型扩展类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public static class NameValueExtension
    {
        /// <summary>
        /// 将键值对集合序列化为Json字符串
        /// </summary>
        /// <param name="collection">键值对集合</param>
        /// <param name="needBracket">是否需要左右括号</param>
        /// <returns></returns>
        public static string ToJson(this NameValueCollection collection, bool needBracket = false)
        {
            if (collection == null) return string.Empty;
            var returnList = new List<string>();
            foreach (var item in collection.AllKeys)
            {
                returnList.Add(string.Format("'{0}':'{1}'", item, collection[item]));
            }
            if (returnList.Count == 0) return string.Empty;
            if (needBracket)
            {
                return "{" + string.Join(",", returnList) + "}";
            }
            else
            {
                return string.Join(",", returnList);
            }
        }

        public static bool IsTrue(this NameValueCollection collection, string key)
        {
            bool isTrue;

            return collection != null
                && collection.GetValues(key) != null
                && bool.TryParse(collection.GetValues(key)[0], out isTrue)
                && isTrue;
        }

        public static bool? IsTrueNullable(this NameValueCollection collection, string key)
        {
            bool? isTrue = null;

            if (collection != null && collection.GetValues(key) != null)
            {
                bool isTrueValue;

                if (bool.TryParse(collection.GetValues(key)[0], out isTrueValue))
                    isTrue = isTrueValue; //FUNNY: (erikpo) Like the hardware store?
            }

            return isTrue;
        }

        public static string ToQueryString(this NameValueCollection queryString)
        {
            if (queryString.Count > 0)
            {
                StringBuilder qs = new StringBuilder();

                qs.Append("?");

                for (int i = 0; i < queryString.Count; i++)
                {
                    if (i > 0)
                        qs.Append("&");

                    qs.AppendFormat("{0}={1}", queryString.Keys[i], queryString[i]);
                }

                return qs.ToString();
            }

            return "";
        }
    }
}
