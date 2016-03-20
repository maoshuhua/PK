using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tuhui.Common45.Mvc;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class ReceptionManagerController : BaseController
    {
        private IReception_UserInfoService _reception_UserInfoService;
        private IReception_ResourceTypeService _reception_ResourceType;

        public ReceptionManagerController()
        {
            _reception_UserInfoService = base.InstanceService<Reception_UserInfoService>();
            _reception_ResourceType = base.InstanceService<Reception_ResourceTypeService>();
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

        public ActionResult ResourceType()
        {
            return View();
        }

        //获取资源列表
        public ActionResult GetResourceTypeList()
        {
            var list = _reception_ResourceType.GetList();

            return Json(list,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceTypeModify(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                @ViewBag.TitleName = "资源分类管理 -> 添加页面";
                return View(new Reception_ResourceType());
            }
            else
            {
                @ViewBag.TitleName = "资源分类管理 -> 编辑页面";
                Reception_ResourceType entity = _reception_ResourceType.Get(id);

                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult ResourceTypeModify(Reception_ResourceType model)
        {
            if (string.IsNullOrEmpty(model.RT_ID))
            {
                model.RT_ID = CommonFun.GenerGuid();
                model.ImgPath = "";
                model.CreateTime = DateTime.Now;
                //添加资源分类
                _reception_ResourceType.Insert(model);
            }
            else {
                //修改资源分类
                _reception_ResourceType.Update(model);
            }

            return RedirectToAction("ResourceType");
        }

        //删除资源分类
        public ActionResult ResourceTypeDelete(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                //抛出异常
            }
            int i = _reception_ResourceType.Delete(id);

            return Json(true);
        }

	}
}