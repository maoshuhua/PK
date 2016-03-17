using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Exception;

namespace Tuhui.Common45.Log
{
    /// <summary>
    /// 日志操作共通类
    /// </summary>
    /// <remarks>
    /// ========================================================================
    /// 更新履历
    /// 序号      修改日期           责任人     更新内容
    /// 001     2011-06-23			 李根华     新建
    /// 002     2010-XX-XX			  ----        ----
    /// ========================================================================
    /// </remarks>
    public static class LoggerManager
    {
        #region 对象变量

        private static string[] loggerNames = new string[] { "SystemExceptionLogger", "UserExceptionLogger", "ControllerActionLogger", "CustomLogger" };

        private static NLogHandle _systemExceptionLogger = new NLogHandle(NLog.LogManager.GetLogger("SystemExceptionLogger"));

        private static NLogHandle _userExceptionLogger = new NLogHandle(NLog.LogManager.GetLogger("UserExceptionLogger"));

        private static NLogHandle _controllerActionLogger = new NLogHandle(NLog.LogManager.GetLogger("ControllerActionLogger"));

        private static NLogHandle _customLogger = new NLogHandle(NLog.LogManager.GetLogger("CustomLogger"));

        #endregion 对象变量

        #region 初始化

        /// <summary>
        /// 静态类构造函数
        /// </summary>
        static LoggerManager()
        {
        }

        #endregion 初始化

        #region ControllerAction相关
        /// <summary>
        /// Action进入信息记录
        /// </summary>
        /// <param name="info"></param>
        public static void Start(string info)
        {
            _controllerActionLogger.Info(info);
        }

        ///// <summary>
        ///// Action进入信息记录
        ///// </summary>
        ///// <param name="model"></param>
        //public static void Start(ControllerActionLogModel model)
        //{
        //    _controllerActionLogger.Info(model.ToStartString());
        //}

        /// <summary>
        /// Action结束信息记录
        /// </summary>
        /// <param name="info"></param>
        public static void End(string info)
        {
            _controllerActionLogger.Info(info);
        }

        ///// <summary>
        ///// Action结束信息记录
        ///// </summary>
        ///// <param name="model"></param>
        //public static void End(ControllerActionLogModel model)
        //{
        //    if (Environment.EnvironmentHandle.GlobalSettings.LogSetting.ActionLog)
        //    {
        //        _controllerActionLogger.Info(model.ToEndString());
        //    }
        //}

        #endregion ControllerAction相关

        #region 异常记录

        /// <summary>
        /// 记录系统异常
        /// </summary>
        /// <param name="ex"></param>
        public static void LogSysException(System.Exception ex)
        {
            _systemExceptionLogger.Fatal("系统异常", ex);
        }

        ///// <summary>
        ///// 记录用户异常
        ///// </summary>
        ///// <param name="ex"></param>
        public static void LogUserException(UserException ex)
        {
            _userExceptionLogger.Error(ex.Message, ex);
        }

        #endregion 异常记录

        #region 自定义信息记录
        /// <summary>
        /// 记录自定义信息
        /// </summary>
        /// <param name="message"></param>
        public static void LogCustomInfo(string message)
        {
            _customLogger.Info(message);
        }

        #endregion 自定义信息记录

    }
}
