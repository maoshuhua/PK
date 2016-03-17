using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：PathUtility
    /// <summary>
    /// 路劲相关常用方法
    /// </summary>
    /// <remarks>
    /// 路劲相关常用方法
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public static class PathUtility
    {
        /// <summary>
        /// 获取当前应用根目录下面的相对地址
        /// </summary>
        /// <param name="virualPath"></param>
        /// <returns></returns>
        public static string GetApplicationPath(string virualPath)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, virualPath);
        }
    }
}
