using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common.Mvc;
using Tuhui.OH.BusinessLogic;
using Tuhui.OH.Entity;
using System.Web.Security;
using Tuhui.OH.IBusinessLogic;
using Tuhui.OH.Model;

namespace Tuhui.OH.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private IUserBusinessLogic _userBL;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_userBL == null)
            {
                _userBL = base.InstanceBusinessLogic<UserBusinessLogic>();
            }

            base.OnActionExecuting(filterContext);
        }

        //
        // GET: /Account/
        public ActionResult LogOn()
        {
            //if (base.LogOnUser != null)
            //{
            //    return RedirectToAction("Index", "Manager");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("", "登录名不能为空");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "密码不能为空");
                return View(model);
            }
            // 如果验证不通过
            if (!_userBL.ValidationUser(model.UserName, model.Password))
            {

                ModelState.AddModelError("", "用户名密码不正确");
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(model.UserName, false);
            SessionHelper.LogOnUser<nz_user>(_userBL.GetUserByUserName(model.UserName));
           
            //_userBL.UpdateUserLogOnLog(SessionHelper.LogOnUser<user>().UserID);
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Manager");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LogOn", "Account");
        }

    }
}
