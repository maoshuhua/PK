using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Tuhui.Common45.Utility;
using Tuhui.Common45.Log;
using Tuhui.Common45.Exception;
using Tuhui.Common45.Environment;

namespace Tuhui.Common45.Mvc
{
    /// =======================================================================
    /// 类名：ExceptionFilter
    /// <summary>
    /// 异常过滤器
    /// </summary>
    /// <remarks>
    /// 异常过滤器
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/10/09        李根华           新建
    /// =======================================================================
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        #region Property & Method
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExceptionFilter()
        {
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            // 输出异常信息到日志
            LogException(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // 判断是否是Ajax异常
                this.ProcessAjaxRequestException(filterContext);
            }
            else
            {
                this.ProcessWebRequestException(filterContext);
            }
        }



        #endregion Property & Method

        #region Private Method

        /// <summary>
        /// 处理页面请求和配置(不显示异常)条件下异常
        /// </summary>
        /// <param name="filterContext"></param>
        private void ProcessWebRequestException(ExceptionContext filterContext)
        {
            string message = "";
            if (filterContext.Exception is UserException)
            {
                message = filterContext.Exception.Message;
            }
            else
            {
                message = "系统发生错误";
            }
            if (!EnvironmentHandle.GlobalSetting.Debugger)
            {
                filterContext.Result = new RedirectResult(EnvironmentHandle.GlobalSetting.ErrorUrl + "?message=" + HttpUtility.UrlEncode(message));
                filterContext.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// 处理AJAX请求异常
        /// </summary>
        /// <param name="filterContext"></param>
        private void ProcessAjaxRequestException(ExceptionContext filterContext)
        {

            if (filterContext.Exception is UserException)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new JsonError("userexception", filterContext.Exception.Message),
                    JsonRequestBehavior = filterContext.HttpContext.Request.RequestType == "GET" ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet
                };
            }
            else
            {
                filterContext.Result = new JsonResult
                {
                    Data = new JsonError("exception", filterContext.Exception.Message),
                    JsonRequestBehavior = filterContext.HttpContext.Request.RequestType == "GET" ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet
                };
            }

            filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// 处理API请求异常
        /// </summary>
        /// <param name="filterContext"></param>
        private void ProcessAPIRequestException(ExceptionContext filterContext)
        {
            //var _userExecption = filterContext.Exception as UserException;
            //if (_userExecption != null)
            //{
            //    filterContext.Result = new ApiResult(_userExecption.MessageID, _userExecption.Message);
            //}
            //else
            //{
            //    filterContext.Result = new ApiResult(CommonCode.ErrorCode.API_SystemUnExceptedError,
            //        MessageHandle.GetMessage(CommonCode.ErrorCode.API_SystemUnExceptedError));
            //}
            //filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="filterContext"></param>
        private void LogException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UserException)
            {
                LoggerManager.LogUserException(filterContext.Exception as UserException);
            }
            else
            {
                LoggerManager.LogSysException(filterContext.Exception);
            }
        }

        #endregion Private Method
    }
}