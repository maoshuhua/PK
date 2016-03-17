using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common45.Mvc;
using Tuhui.Reception.Model;
using Tuhui.Reception.Service;

namespace Tuhui.Reception.WebUI.Controllers
{
    public class HomeController : BaseController<tv_user>
    {
        private IFaceService _faceService;
        public HomeController()
        {
            _faceService = base.InstanceService<FaceService>();
        }
        public ActionResult Index()
        {
            return View(_faceService.GetTVFaceList(null));
        }

        public ActionResult ToolGD() {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Statistic()
        {
            return View();
        }

        public ActionResult BusLine(string lineCode, string directCode)
        {

            if (string.IsNullOrWhiteSpace(lineCode) || string.IsNullOrWhiteSpace(directCode))
            {
                return Content("参数不能为空");
            }

            var _list = _faceService.GetBusLine(lineCode, directCode);

            return Content("<textarea style='width:500px;height:500px;margin:10px auto;'>"+string.Join(";", _list.Select(p => p.lng + "," + p.lat).ToList())+"</textarea>");

        }
    }
}