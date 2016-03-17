using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.IO;
using Tuhui.Common45.Environment;
using Tuhui.Common45.Utility;
using System.Web.Routing;
using System.Web;

namespace Tuhui.Common45.Mvc
{
    public class THCommController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.ActionDescriptor.ActionName.ToLower() != "authorize")
            //{
            //    if (SessionHelper.Get(CommonCode.GlobalContant.SessionKey_THComm_Authorize) == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //        {
            //            controller = "THComm",
            //            action = "Authorize"
            //        }));
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return Content("the system has restarted!");
        }

        public ActionResult Authorize()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SysInfo()
        {
            return View(new SysProp());
        }

        public ActionResult CacheSetting()
        {
            return View();
        }

        public ActionResult GlobalSetting()
        {
            return View();
        }

        public ActionResult LogSetting()
        {
            return View();
        }

        public ActionResult MessageSetting()
        {
            return View();
        }
    }
}
