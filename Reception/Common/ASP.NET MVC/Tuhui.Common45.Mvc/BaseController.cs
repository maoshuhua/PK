using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tuhui.Common45.Utility;
using Tuhui.Common45.Framework;
using Tuhui.Common45.Environment;

namespace Tuhui.Common45.Mvc
{
    /// <summary>
    /// MVC框架Controller类基类
    /// </summary>
    public class BaseController : BaseController<object>
    {

    }

    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController<T> : Controller where T : class
    {
        #region 接口实例化
        /// <summary>
        /// 实例化BL接口
        /// </summary>
        /// <typeparam name="BL"></typeparam>
        /// <returns></returns>
        public BL InstanceService<BL>() where BL : BaseService, new()
        {
            return new BL();
        }
        #endregion

        #region 登陆信息
        /// <summary>
        /// 返回登陆者信息
        /// </summary>
        public T LogOnUser
        {
            get
            {
                return SessionHelper.LogOnUser<T>();
            }
        }

        /// <summary>
        /// 设置登陆者信息
        /// </summary>
        /// <param name="_user"></param>
        public void SetLogOnUser(T _user)
        {
            SessionHelper.LogOnUser<T>(_user);
        }
        #endregion

        #region ViewResult
        /// <summary>
        /// Creates a System.Web.Mvc.ViewResult object that renders a view to the response.
        /// </summary>
        /// <returns>The view result that renders a view to the response.</returns>
        public ViewResult TView()
        {
            return View();
        }

        /// <summary>
        /// Creates a System.Web.Mvc.ViewResult object by using the model that renders
        //     a view to the response.
        /// </summary>
        /// <param name="model">The model that is rendered by the view.</param>
        /// <returns>The view result.</returns>
        public ViewResult TView(object model)
        {
            return View(model);
        }

        #endregion

        #region JsonResult
        public ActionResult Json(ModelStateDictionary modelState)
        {
            var keys = modelState.Keys;
            var diction = new Dictionary<string, string>();
            foreach (var item in keys)
            {
                if (modelState[item].Errors.Count == 0) continue;
                if (!string.IsNullOrEmpty(item))
                {
                    diction.Add(item, modelState[item].Errors[0].ErrorMessage);
                }
            }
            return Json(new JsonError("modelstate", diction));
        }
        #endregion

        #region 其他
        public ActionResult UserException(string messageId, params object[] parameters)
        {
            return new UserExceptionResult(messageId, parameters);
        }

        public string GetClientIP()
        {
            string ip;
            if (Request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }
        #endregion

        #region 序列化
        public string ToJsonString(object obj)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
        }
        #endregion
    }
}
