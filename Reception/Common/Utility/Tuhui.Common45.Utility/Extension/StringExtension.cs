using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    public static class StringExtension
    {
        /// <summary>
        /// 进行字符串分隔符形式进行查询
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="value">包含字符串</param>
        /// <param name="split">分隔符</param>
        /// <param name="or">与还是或操作  默认是或操作</param>
        /// <returns></returns>
        public static bool Contains(this String str, string value, char split, bool or = true)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(value)) return false;
            var _valueArr = value.Split(split);
            foreach (var item in _valueArr)
            {
                if (or && str.Contains(str))
                {
                    return true;
                }
                if (!or && !str.Contains(str))
                {
                    return false;
                }
            }
            return !or;
        } 

        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        /// <summary>
        /// 去除尾部多余的特定字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="subOther"></param>
        /// <returns></returns>
        public static string TrimEndString( this String inputString, string subOther )
        {
            if ( String.IsNullOrEmpty( inputString ) )
                return inputString;
            if ( inputString.Trim().Length == 0 && subOther.Trim().Length > 0 )
                return inputString;
            var temp = inputString;
            while ( temp.LastIndexOf( subOther ) >= 0 ) {
                temp = temp.Substring( 0, temp.LastIndexOf( subOther ) );
            }
            return temp;
        }

         

    }
}
