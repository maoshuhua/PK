using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.OH.BusinessLogic;

namespace Tuhui.OH.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private NoticeBusinessLogic _noticeBusinessLogic = new NoticeBusinessLogic();
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 右侧菜单栏
        /// </summary>
        /// <returns></returns>
        public ActionResult RightMenu()
        {
            return View();
        }

        /// <summary>
        /// 当前位置
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentPosition()
        {
            return View();
        }

        /// <summary>
        /// 分层浏览页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FindLayer()
        {
            return View();
        }

        /// <summary>
        /// 交通换乘
        /// </summary>
        /// <returns></returns>
        public ActionResult TrafficTransfer()
        {
            return View();
        }

        /// <summary>
        /// 售票处
        /// </summary>
        /// <returns></returns>
        public ActionResult FindTicket()
        {
            return View();
        }

        /// <summary>
        /// 进站口
        /// </summary>
        /// <returns></returns>
        public ActionResult FindEntrance()
        {
            return View();
        }

        /// <summary>
        /// 出站口
        /// </summary>
        /// <returns></returns>
        public ActionResult FindExit()
        {
            return View();
        }

        /// <summary>
        /// 餐饮区
        /// </summary>
        /// <returns></returns>
        public ActionResult FindCater()
        {
            return View();
        }

        /// <summary>
        /// 卫生间
        /// </summary>
        /// <returns></returns>
        public ActionResult FindToilet()
        {
            return View();
        }

        /// <summary>
        /// 南站公告
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowNotice()
        {
            return View();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnOn()
        {
            return View();
        }


        /// <summary>
        /// 模拟导航
        /// </summary>
        /// <returns></returns>
        public ActionResult AnalogNavigation()
        {
            return View();
        }

        /// <summary>
        /// 导航距离时间信息显示
        /// </summary>
        /// <returns></returns>
        public ActionResult NavInfo()
        {
            return View();
        }

        /// <summary>
        /// 公交车
        /// </summary>
        /// <returns></returns>
        public ActionResult FindBus()
        {
            return View();
        }

        /// <summary>
        /// 查询公告信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchNoticeList()
        {
            var result = _noticeBusinessLogic.GetNoticeList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 交通换乘 路线图显示
        /// </summary>
        /// <returns></returns>
        public ActionResult TrafficRoute()
        {
            return View();
        }

        public ActionResult ShowNoticeDetail()
        {
            return View();
        }


        /// <summary>
        /// 消除图标消失
        /// </summary>
        /// <returns></returns>
        public ActionResult _refresh()
        {
            return View();
        }

        /// <summary>
        /// 点击摄像头 播放视频
        /// </summary>
        /// <returns></returns>
        public ActionResult PlayVideo()
        {
            return View();
        }

    }
}
