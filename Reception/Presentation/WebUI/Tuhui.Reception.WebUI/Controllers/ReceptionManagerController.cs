using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tuhui.Common45.Mvc;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class ReceptionManagerController : BaseController
    {
        private IReception_UserInfoService _reception_UserInfoService;
        public ReceptionManagerController()
        {
            _reception_UserInfoService = base.InstanceService<Reception_UserInfoService>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ReceptionManager");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(Reception_UserInfo model, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("", "登录名不能为空");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.Pwd))
            {
                ModelState.AddModelError("", "密码不能为空");
                return View(model);
            }
            // 如果验证不通过
            if (!_reception_UserInfoService.ValidationUser(model))
            {

                ModelState.AddModelError("", "用户名密码不正确");
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(model.Name, false);

            var _user = _reception_UserInfoService.GetUserByUserName(model.Name);

            SessionHelper.LogOnUser<Reception_UserInfo>(_user);
            
            return RedirectToAction("Index", "ReceptionManager");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LogOn", "ReceptionManager");
        }

	}
}