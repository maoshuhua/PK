using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Tuhui.Reception.WebUI.Models;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;
using Tuhui.Common45.Mvc;
using System.Web.Security;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private IUserService _userService;
        public AccountController()
        {
            _userService = base.InstanceService<UserService>();
        }
        public ActionResult LogOn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manager");
            }
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
            if (!_userService.ValidationUser(model.UserName, model.Password))
            {

                ModelState.AddModelError("", "用户名密码不正确");
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(model.UserName, false);

            var _user = _userService.GetUserByUserName(model.UserName);

            SessionHelper.LogOnUser<tv_user>(_user);

            return RedirectToAction("Index", "Manager");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Manager");
        }
    }
}