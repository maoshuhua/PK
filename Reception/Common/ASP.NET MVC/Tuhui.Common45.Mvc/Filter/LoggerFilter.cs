using System;
using System.Web.Mvc;
using Tuhui.Common45.Log;
using Tuhui.Common45.Utility;

namespace Tuhui.Common45.Mvc
{
    /// =======================================================================
    /// 类名：LoggerFilter
    /// <summary>
    /// Action进出日志记录
    /// </summary>
    /// <remarks>
    /// Action进出日志记录
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/10/09        李根华           新建
    /// =======================================================================
    public class LoggerFilter : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Action进入进行记录
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoggerManager.Start("Start_Action:" + new ControllerActionLogModel
            {
                OccurTime = DateTime.Now,
                ActionName = filterContext.ActionDescriptor.ActionName,
                ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Method = filterContext.HttpContext.Request.HttpMethod,
                Url = filterContext.HttpContext.Request.Url.ToString(),
                Browser = filterContext.HttpContext.Request.Browser == null
                   ? string.Empty
                   : filterContext.HttpContext.Request.Browser.Browser + "(V"
                   + filterContext.HttpContext.Request.Browser.MajorVersion + ")",
                Form = filterContext.HttpContext.Request.Form.ToJson(true),
                QueryString = filterContext.HttpContext.Request.QueryString.ToJson(true),
                UserHostAddress = filterContext.HttpContext.Request.UserHostAddress,
                UserHostName = filterContext.HttpContext.Request.UserHostName
            }.ToString());
        }


        /// <summary>
        /// Action结束进行日志记录
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            LoggerManager.End("End_Action:" + new ControllerActionLogModel
            {
                OccurTime = DateTime.Now,
                ActionName = filterContext.ActionDescriptor.ActionName,
                ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
            }.ToShortString());
        }
    }
}