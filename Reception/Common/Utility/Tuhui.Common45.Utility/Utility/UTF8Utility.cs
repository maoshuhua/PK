using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    public static class UTF8Utility
    {
        /// =======================================================================
        /// 方法名：ToUTF8
        /// <summary>
        /// =======================================================================
        /// 转换成UTF8编码
        /// =======================================================================
        /// 更新履历
        /// 序号          修改日期          责任人           更新内容
        /// 001           2014/04/22        佚名           新建
        /// =======================================================================
        /// </summary>
        /// <remarks>
        /// ToUTF8的备注：
        /// </remarks>
        /// <param name="frm"></param>
        /// <returns>ToUTF8的返回值：</returns>
        /// =======================================================================
        public static string ToUTF8(this String frm)
        {
            if (string.IsNullOrWhiteSpace(frm)) return string.Empty;

            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(frm);
            string str = "";

            foreach (byte b in buffer) str += string.Format("%{0:X}", b);
            return str;
        }
    }
}
