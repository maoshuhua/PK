using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Log
{
    /// <summary>
    /// 使用NLog.dll，实现ILogger接口的适配类
    /// </summary>
    /// <remarks>
    /// ========================================================================
    /// 更新履历
    /// 序号      修改日期           责任人     更新内容
    /// 001     2011-06-22			 李根华     新建
    /// 002     2010-XX-XX			  ----        ----
    /// ========================================================================
    /// </remarks>
    internal class NLogHandle
    {
        #region 变量/构造函数
        private Logger _log;

        public NLogHandle(Logger log)
        {
            LogManager.Configuration = new XmlLoggingConfiguration(PathUtility.GetApplicationPath(CommonCode.ConfigName.NLogConfig));
            this._log = log;
        }

        #endregion

        #region ILogger接口实现

        public bool IsDebugEnabled
        {
            get { return this._log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this._log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return this._log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return this._log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return this._log.IsFatalEnabled; }
        }

        public void Debug(string message)
        {
            this._log.Debug(message);
        }

        public void Debug(string message, System.Exception exception)
        {
            this._log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] arg0)
        {
            this._log.Debug(format, arg0);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._log.Debug(provider, format, args);
        }

        public void Info(string message)
        {
            this._log.Info(message);
        }

        public void Info(string message, System.Exception exception)
        {
            this._log.Info(message, exception);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this._log.Info(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this._log.Info(format, args);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._log.Info(provider, format, args);
        }

        public void Warn(string message)
        {
            this._log.Warn(message);
        }

        public void Warn(string message, System.Exception exception)
        {
            this._log.Warn(message, exception);
        }
      
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this._log.Warn(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this._log.Warn(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._log.Warn(provider, format, args);
        }

        public void Error(string message)
        {
            this._log.Error(message);
        }

        public void Error(string message, System.Exception exception)
        {
            this._log.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this._log.Error(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this._log.Error(format, args);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._log.Error(provider, format, args);
        }

        public void Fatal(string message)
        {
            this._log.Fatal(message);
        }

        public void Fatal(string message, System.Exception exception)
        {
            this._log.Fatal(message, exception);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this._log.Fatal(format, arg0, arg1, arg2);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this._log.Fatal(format, args);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._log.Fatal(provider, format, args);
        }

        #endregion
    }
}
