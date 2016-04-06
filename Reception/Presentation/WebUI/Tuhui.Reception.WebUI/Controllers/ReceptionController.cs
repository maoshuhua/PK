using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common45.Mvc;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class ReceptionController : BaseController
    {
        private IReception_ResourceTypeService _reception_ResourceType;
        private IReception_ResourceService _reception_Resource;
        private IImageService _image;
        private IVideoService _video;
        private IReception_ResourceEventService _event;
        private INavigationService _navigation;

        public ReceptionController()
        {
            _reception_ResourceType = base.InstanceService<Reception_ResourceTypeService>();
            _reception_Resource = base.InstanceService<Reception_ResourceService>();
            _image = base.InstanceService<ImageService>();
            _video = base.InstanceService<VideoService>();
            _event = base.InstanceService<Reception_ResourceEventService>();
            _navigation = base.InstanceService<NavigationService>();
        }

        public ActionResult Index()
        {
            return View();
        }

        //获取所有资源
        public ActionResult GetResourceList()
        {
            var list = _reception_Resource.GetList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取最新一条导航
        public ActionResult GetNewest()
        {
            var entity = _navigation.GetNewest();

            return Json(entity,JsonRequestBehavior.AllowGet);
        }

        //获取资源分类及其资源列表
        public ActionResult GetResource_Type()
        {
            return Json(_reception_Resource.GetResource_Type(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceDetail()
        {
            ViewBag.Id = Request.QueryString["id"];
            return View();
        }

        public ActionResult EventDetail()
        {
            return View();
        }

        //获取资源详情
        public ActionResult GetResourceInfo(string id)
        {
            var entity = _reception_Resource.Get(id);

            return Json(entity,JsonRequestBehavior.AllowGet);
        }

        //获取资源图片
        public ActionResult GetResourceImage(string id)
        {
            var list = _image.GetList(id);

            return Json(list,JsonRequestBehavior.AllowGet);
        }

        //获取资源视频

        //获取资源下大事件列表

	}
}