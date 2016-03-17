using System;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：CommonCode
    /// <summary>
    /// 全局常量及枚举定义
    /// </summary>
    /// <remarks>
    /// 全局常量及枚举定义
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2014/08/15        李根华           新建
    /// =======================================================================
    public partial class CommonCode
    {
        #region 日志类型 
        /// <summary>
        /// 日志类型 
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 系统错误
            /// </summary>
            SystemError,

            /// <summary>
            /// 自定义错误
            /// </summary>
            CustomError,

            /// <summary>
            /// Controller调用日志
            /// </summary>
            Controller,

            /// <summary>
            /// 自定义日志
            /// </summary>
            Custom,

            /// <summary>
            /// Sql跟踪日志
            /// </summary>
            SqlTrace,

            /// <summary>
            /// Email
            /// </summary>
            Email,

            /// <summary>
            /// 短信
            /// </summary>
            SMS
        }
        #endregion 

    }
}