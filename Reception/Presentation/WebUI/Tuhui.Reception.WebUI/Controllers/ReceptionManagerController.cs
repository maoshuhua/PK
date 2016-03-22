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
        private IReception_ResourceService _reception_Resource;
        private IImageService _image;
        private IVideoService _video;

        public ReceptionManagerController()
        {
            _reception_UserInfoService = base.InstanceService<Reception_UserInfoService>();
            _reception_ResourceType = base.InstanceService<Reception_ResourceTypeService>();
            _reception_Resource = base.InstanceService<Reception_ResourceService>();
            _image = base.InstanceService<ImageService>();
            _video = base.InstanceService<VideoService>();
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

        public ActionResult Resource()
        {
            return View();
        }

        //资源列表
        public ActionResult GetResourcePageList(int pageIndex = 1, int pageSize = 10)
        {
            var list = _reception_Resource.GetPageList(null, pageIndex, pageSize);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceModify(string id)
        {
            List<Reception_ResourceType> resourceTypeList = _reception_ResourceType.GetList();
            resourceTypeList.Insert(0, new Reception_ResourceType { 
                RT_ID = "",
                Name = "--请选择--"
            });
            ViewData["resourceTypeList"] = new SelectList(resourceTypeList, "RT_ID", "Name");

            if (string.IsNullOrEmpty(id))
            {
                @ViewBag.TitleName = "资源管理 -> 添加页面";
                Reception_Resource entity = new Reception_Resource();
                entity.StartTime = DateTime.Now;
                entity.EndTime = DateTime.Now;

                return View(entity);
            }
            else
            {
                @ViewBag.TitleName = "资源管理 -> 编辑页面";
                Reception_Resource entity = _reception_Resource.Get(id);

                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult ResourceModify(Reception_Resource model)
        {
            if (string.IsNullOrEmpty(model.R_ID))
            {
                model.R_ID = CommonFun.GenerGuid();
                model.AddTime = DateTime.Now;
                //添加资源
                _reception_Resource.Insert(model);
            }
            else
            {
                //修改资源
                _reception_Resource.Update(model);
            }

            return RedirectToAction("Resource");
        }

        //删除资源分类
        public ActionResult ResourceDelete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                //抛出异常
            }
            //删除资源
            _reception_Resource.Delete(id);
            //删除资源图片
            _image.DeleteList(id);
            //删除资源视频
            _video.DeleteList(id);

            return Json(true);
        }

	}
}