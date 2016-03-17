using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuhui.Common45.Mvc;
using Tuhui.Common45.Utility;
using Tuhui.Reception.Model;
using Tuhui.Reception.Mvc;
using Tuhui.Reception.Service;
using Tuhui.Reception.Utility;

namespace Tuhui.Reception.WebUI.Controllers
{

    //public class ManagerController : BaseController<tv_user>
    public partial class ManagerController : BaseController<tv_user>
    {
        private IVideoService _videoService;
        private IUserService _userService;
        private IUserService _logService;
        private IFaceService _faceService;


        public ManagerController()
        {
            _videoService = base.InstanceService<VideoService>();
            _userService = base.InstanceService<UserService>();
            _logService = base.InstanceService<UserService>();
            _faceService = base.InstanceService<FaceService>();
        }

        [AuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult VideoList()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult SubjectList()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult FaceList()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult Sync()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult UserList()
        {
            return View();
        }

        [AuthorizeFilter]
        public ActionResult LogList()
        {
            return View();
        }

        public ActionResult JTMap()
        {
            return View();
        }

        public ActionResult VideoModify(int? videoid)
        {
            if (videoid == null)
            {
                @ViewBag.TitleName = "视频管理 -> 添加页面";
                return View(new tv_video());
            }
            else
            {
                @ViewBag.TitleName = "视频管理 -> 编辑页面";
                return View(_videoService.GetTV_VideoById(videoid.Value));
            }
        }

        [HttpPost]
        public ActionResult VideoModify(tv_video model)
        {
            if (model.videoid == 0)
            {
                _videoService.AddVideo(model);
            }
            else
            {
                _videoService.EditVideoById(model.videoid, model);
            }
            return RedirectToAction("VideoList");
        }

        public ActionResult LogModify(int? logid)
        {
            if (logid == null)
            {
                @ViewBag.TitleName = "日志管理 -> 添加页面";
                return View(new tv_log());
            }
            else
            {
                @ViewBag.TitleName = "日志管理 -> 编辑页面";
                return View(_logService.GetTV_LogById(logid.Value));
            }
        }

        [HttpPost]
        public ActionResult LogModify(tv_log model)
        {
            if (model.logid == 0)
            {
                model.logtime = DateTime.Now;
                _logService.AddLog(model);
            }
            else
            {
                model.logtime = DateTime.Now;
                _logService.EditLogById(model.logid, model);
            }
            return RedirectToAction("LogList");
        }

        public ActionResult FaceModify(int? faceid)
        {
            ViewBag.ImageList =
                new System.IO.DirectoryInfo(Server.MapPath("~/Images/face")).GetFiles().Select(p => p.Name).ToList();
            ViewBag.VideoList = _videoService.GetTV_VideoList(null).ToList();

            if (faceid == null)
            {
                @ViewBag.TitleName = "驾驶舱管理 -> 添加页面";
                return View(new tv_face { sortno = 20 });
            }
            else
            {
                @ViewBag.TitleName = "驾驶舱管理 -> 编辑页面";
                return View(_faceService.GetTV_FaceById(faceid.Value));
            }
        }

        [HttpPost]
        public ActionResult FaceModify(tv_face model)
        {
            if (model.faceid == 0)
            {
                _faceService.AddFace(model);
            }
            else
            {
                _faceService.EditFaceById(model.faceid, model);
            }
            return RedirectToAction("FaceList");
        }

        public ActionResult UserModify(int? userid)
        {
            if (userid == null)
            {
                @ViewBag.TitleName = "用户管理 -> 添加页面";
                return View(new tv_user());
            }
            else
            {
                @ViewBag.TitleName = "用户管理 -> 编辑页面";
                return View(_userService.GetUserByUserId(userid.Value));
            }
        }

        [HttpPost]
        public ActionResult UserModify(tv_user model)
        {
            if (model.userid == 0)
            {
                model.lastloginIP = "192.168.11.120";
                model.createtime = DateTime.Now;
                model.lastlogintime = DateTime.Now;
                _userService.AddUser(model);
            }
            else
            {
                model.lastloginIP = "192.168.11.121";
                model.lastlogintime = DateTime.Now;
                _userService.EditUserById(model.userid, model);
            }
            return RedirectToAction("UserList");
        }

        public ActionResult ProvinceVideoApiInsert()
        {
            var _result = WebRequestHandle.Get(Constant.ProvinceVideoResourceAPI);
            if (_result.Success == false)
            {
                return Content("Error: Api Error!<br/> 地址:【" + Constant.ProvinceVideoResourceAPI + "】");
            }
            else
            {
                var aaa = this.SerializeXMLFromProvinceAPI(_result.Response);
                foreach (var item in aaa)
                {
                    _videoService.AddVideo(item);
                }
                return Content("成功新增" + aaa.Count + "条记录");
            }
        }


        #region 云创视频更新
        public ActionResult YCVideoUpdate()
        {
            return View();
        }
        #endregion
    }

}